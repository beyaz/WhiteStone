using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for reflection operations.
    /// </summary>
    public static partial class ReflectionHelper
    {
        #region Public Properties
        /// <summary>
        ///     BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
        ///     BindingFlags.Static
        /// </summary>
        public static BindingFlags AllBindings
        {
            get { return BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Perform a deep Copy of the object.
        /// </summary>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream     stream    = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///     Copies all properties to <paramref name="destination" /> instance.
        /// </summary>
        public static T CopyProperties<T>(this DataRow dataRow, T destination) where T : class
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException("dataRow");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            foreach (var targetProperty in destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var columnIndex = dataRow.Table.Columns.IndexOf(targetProperty.Name);

                if (columnIndex < 0)
                {
                    continue;
                }

                TrySetPropertyValue(targetProperty, destination, dataRow[targetProperty.Name]);
            }

            return destination;
        }

        public static void CopyProperties<T>(T source, T destination) where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p=>p.CanRead && p.CanWrite))
            {
                propertyInfo.SetValue(destination,propertyInfo.GetValue(source));
            }
        }

        /// <summary>
        ///     Exports <paramref name="instance" /> to c# code initialization
        /// </summary>
        public static string ExportObjectToCSharpCode(object instance)
        {
            return new ObjectToCSharpCodeExporter().Export(instance);
        }

        /// <summary>
        ///     Gets default value of <paramref name="type" />
        /// </summary>
        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        public static MethodInfo GetMethod(string methodAsSerializedString)
        {
            const string Message = "Format must be 'Type::methodName,assemblyName'";

            var startMethod = methodAsSerializedString.IndexOf("::", StringComparison.Ordinal);
            if (startMethod < 0)
            {
                throw new ArgumentException(Message);
            }

            var endMethod = methodAsSerializedString.IndexOf(",", startMethod, StringComparison.Ordinal);
            if (endMethod < 0)
            {
                throw new ArgumentException(Message);
            }

            var methodName = methodAsSerializedString.Substring(startMethod + 2, endMethod - startMethod - 2);

            var typeName = methodAsSerializedString.Replace("::" + methodName, string.Empty);

            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException(Message + " type not found.");
            }

            return type.GetMethod(methodName, AllBindings);
        }

        /// <summary>
        ///     Gets the public non static field.
        /// </summary>
        public static FieldInfo GetPublicNonStaticField(this Type type, string fieldName)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            return type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        ///     Gets the public non static property.
        /// </summary>
        public static PropertyInfo GetPublicNonStaticProperty(this Type type, string propertyName,bool throwExceptionOnNotFound = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var property =  type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null && throwExceptionOnNotFound)
            {
                throw  new MissingMemberException(type.FullName + propertyName);
            }

            return property;
        }

        /// <summary>
        ///     Reads embeded resource as string.
        /// </summary>
        public static string ReadEmbeddedResourceAsString(this Assembly assembly, string resourceNameLike)
        {
            var resourceName = assembly.GetManifestResourceNames().First(x => x.Contains(resourceNameLike));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     Returns rows as given type.
        /// </summary>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            if (dt == null)
            {
                throw new ArgumentNullException("dt");
            }

            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var instance = CopyProperties(row, new T());
                list.Add(instance);
            }

            return list;
        }

        /// <summary>
        ///     Returns data table row values as given type.
        /// </summary>
        public static IList<T> ToList<T>(this DataColumn dc)
        {
            if (dc == null)
            {
                throw new ArgumentNullException("dc");
            }

            var list = new List<T>();
            foreach (DataRow row in dc.Table.Rows)
            {
                var value = row[dc];

                var castedValue = Cast.To<T>(value);

                list.Add(castedValue);
            }

            return list;
        }

        /// <summary>
        ///     Returns true if operations is successfull else is value type is not suitable for property then returns false
        /// </summary>
        public static bool TrySetPropertyValue(PropertyInfo targetProperty, object instance, object value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (targetProperty == null)
            {
                throw new ArgumentNullException("targetProperty");
            }

            if (!targetProperty.CanWrite)
            {
                return false;
            }

            if (targetProperty.PropertyType != typeof(DBNull) && value != null && value.Equals(DBNull.Value))
            {
                value = null;
            }

            var isAssignable = false;

            if (value != null)
            {
                isAssignable = targetProperty.PropertyType.IsInstanceOfType(value);
            }

            isAssignable |= value == null && targetProperty.PropertyType.IsClass;

            if (!isAssignable)
            {
                var convertibleValue = value as IConvertible;
                if (convertibleValue != null)
                {
                    var targetPropertyTypeUnderlyingType = Nullable.GetUnderlyingType(targetProperty.PropertyType);

                    if (targetPropertyTypeUnderlyingType == null)
                    {
                        value = convertibleValue.ToType(targetProperty.PropertyType, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var targetIsDate = targetPropertyTypeUnderlyingType == typeof(DateTime);

                        value = targetIsDate ? convertibleValue.To<DateTime>() : convertibleValue.ToType(targetPropertyTypeUnderlyingType, CultureInfo.InvariantCulture);

                        value = Activator.CreateInstance(targetProperty.PropertyType, value);
                    }

                    isAssignable = true;
                }
            }

            if (!isAssignable)
            {
                return false;
            }

            targetProperty.SetValue(instance, value, null);
            return true;
        }
        #endregion
    }
}