using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using BOA.Common.Helpers;
using WhiteStone.Common;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Utility methods for reflection operations.
    /// </summary>
    public static partial class ReflectionUtility
    {
        /// <summary>
        ///     BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
        ///     BindingFlags.Static
        /// </summary>
        public static BindingFlags AllBindings
        {
            get { return BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static; }
        }

        /// <summary>
        ///     Copies all properties to <paramref name="destination" /> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Returns rows as given type.
        /// </summary>
        public static IList<T> ToList<T>(this DataTable dt) where T : class, new()
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
        /// <param name="targetProperty">Not null</param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets default value of <paramref name="type" />
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        

        /// <summary>
        ///     Exports <paramref name="instance" /> to c# code initialization
        /// </summary>
        public static string ExportObjectToCSharpCode(object instance)
        {
            return new ObjectToCSharpCodeExporter().Export(instance);
        }

        class ObjectToCSharpCodeExporter
        {
            int _currentPadding;
            readonly int _padding = 4;
            readonly StringBuilder _sb = new StringBuilder();

            static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

            void Write(object obj)
            {
                if (obj == null)
                {
                    AppendNoPadding("null");
                    return;
                }

                var str = obj as string;
                if (str != null)
                {
                    AppendNoPadding("\"" + str + "\"");
                    return;
                }

                if (obj is sbyte ||
                    obj is byte ||
                    obj is short ||
                    obj is ushort ||
                    obj is int ||
                    obj is double ||
                    obj is uint ||
                    obj is long ||
                    obj is ulong)
                {
                    AppendNoPadding(obj.ToString());
                    return;
                }

                if (obj is decimal)
                {
                    AppendNoPadding(obj + "M");
                    return;
                }

                if (obj is bool)
                {
                    AppendNoPadding(obj.ToString().ToLower(EnglishCulture));
                    return;
                }

                if (obj is float)
                {
                    AppendNoPadding(obj + "f");
                    return;
                }

                if (obj is Guid)
                {
                    var guid = (Guid) obj;
                    if (guid == Guid.Empty)
                    {
                        AppendNoPadding("System.Guid.Empty");
                        return;
                    }
                    AppendNoPadding("new System.Guid(\"" + guid + "\")");
                    return;
                }

                if (obj is DateTime)
                {
                    var date = (DateTime) obj;
                    AppendNoPadding(string.Format(EnglishCulture,"new System.DateTime({0},{1},{2},{3},{4},{5},{6})", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond));
                    return;
                }

                if (obj is Enum)
                {
                    AppendNoPadding(obj.GetType().FullName + "." + obj);
                    return;
                }

                var type = obj.GetType();

                if (IsInstanceofGeneric(typeof(KeyValuePair<,>), obj))
                {
                    var fullName = type.ToString().Replace("`2[", "<").Replace("]", ">");
                    AppendNoPadding(fullName);
                    AppendNoPadding("(");

                    var key = type.GetProperty("Key").GetValue(obj, null);
                    var value = type.GetProperty("Value").GetValue(obj, null);
                    Write(key);
                    AppendNoPadding(",");
                    Write(value);

                    AppendNoPadding(")");
                    return;
                }

                var isFirstPropertWrite = true;

                AppendNoPadding("new ");

                if (IsInstanceofGeneric(typeof(List<>), obj))
                {
                    var fullName = type.ToString().Replace("`1[", "<").Replace("]", ">");

                    AppendNoPadding(fullName);
                    AppendLine();
                    AppendLine("{");

                    PaddingNext();

                    foreach (var item in (IList) obj)
                    {
                        if (isFirstPropertWrite)
                        {
                            isFirstPropertWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                            AppendLine();
                        }

                        WritePadding();
                        Write(item);
                    }

                    PaddingBack();
                    AppendLine();
                    Append("}");

                    return;
                }

                if (IsInstanceofGeneric(typeof(Dictionary<,>), obj))
                {
                    var fullName = type.ToString().Replace("`2[", "<").Replace("]", ">");

                    AppendNoPadding(fullName);
                    AppendLine();
                    AppendLine("{");

                    PaddingNext();

                    foreach (DictionaryEntry item in (IDictionary) obj)
                    {
                        if (isFirstPropertWrite)
                        {
                            isFirstPropertWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                            AppendLine();
                        }

                        WritePadding();
                        AppendNoPadding("{");
                        Write(item.Key);
                        AppendNoPadding(",");
                        Write(item.Value);
                        AppendNoPadding("}");
                    }

                    PaddingBack();
                    AppendLine();
                    Append("}");

                    return;
                }

                AppendNoPadding(type.FullName);
                AppendLine();
                Append("{");

                PaddingNext();

                var array = obj as Array;
                if (array != null)
                {
                    foreach (var value in array)
                    {
                        if (isFirstPropertWrite)
                        {
                            isFirstPropertWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                            AppendLine();
                        }

                        Write(value);
                    }
                }
                else
                {
                    foreach (var property in type.GetProperties().Where(p => p.CanRead && p.CanWrite))
                    {
                        var value = property.GetValue(obj, null);
                        if (value == null)
                        {
                            continue;
                        }
                        if (value.Equals(GetDefaultValueFromType(property.PropertyType)))
                        {
                            continue;
                        }

                        if (property.PropertyType == typeof(DateTime))
                        {
                            if (value.Equals(new DateTime()))
                            {
                                continue;
                            }
                        }
                        if (isFirstPropertWrite)
                        {
                            isFirstPropertWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                        }

                        AppendLine();

                        Append(property.Name);
                        AppendNoPadding(" = ");

                        Write(value);
                    }
                }
                PaddingBack();

                AppendLine();
                Append("}");
            }

            void PaddingNext()
            {
                _currentPadding += _padding;
            }

            void PaddingBack()
            {
                _currentPadding -= _padding;
            }

            void Append(string value)
            {
                WritePadding();
                _sb.Append(value);
            }

            /// <summary>
            ///     Return true if given <paramref name="value" /> is instanceOf <paramref name="targetType" />
            /// </summary>
            static bool IsInstanceofGeneric(Type targetType, object value)
            {
                if (value == null)
                {
                    return false;
                }

                var oType = value.GetType();

                if (oType.IsGenericType && oType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }
                return false;
            }

            void WritePadding()
            {
                if (_currentPadding > 0)
                {
                    _sb.Append("".PadRight(_currentPadding, ' '));
                }
            }

            void AppendNoPadding(string value)
            {
                _sb.Append(value);
            }

            void AppendLine(string value = "")
            {
                WritePadding();
                _sb.AppendLine(value);
            }

            internal string Export(object obj)
            {
                Write(obj);

                return _sb.ToString();
            }

            /// <summary>
            ///     Gets default value of <paramref name="type" />
            /// </summary>
            static object GetDefaultValueFromType(Type type)
            {
                if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                {
                    return Activator.CreateInstance(type);
                }
                return null;
            }
        }

        /// <summary>
        ///     Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
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
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
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
    }
}