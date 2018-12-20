using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CustomUIMarkupLanguage
{
    /// <summary>
    ///     The extensions
    /// </summary>
    static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Gets the field.
        /// </summary>
        public static FieldInfo GetField(this Type type, string fieldName, bool? isPublic, bool? isStatic, bool ignoreCase, bool throwExceptionOnNotFound)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            FieldInfo fieldInfo = null;

            var fields = type.GetFields(GetFlag(isPublic, isStatic));

            if (ignoreCase)
            {
                fieldInfo = fields.FirstOrDefault(p => p.Name.ToUpperEN() == fieldName.ToUpperEN());
            }
            else
            {
                fieldInfo = fields.FirstOrDefault(p => p.Name == fieldName);
            }

            if (fieldInfo == null && throwExceptionOnNotFound)
            {
                throw new MissingMemberException(type.FullName + "::" + fieldName);
            }

            return fieldInfo;
        }

        /// <summary>
        ///     Gets the get method.
        /// </summary>
        public static MethodInfo GetMethod(this Type type, string methodName, bool? isPublic, bool? isStatic, bool ignoreCase, bool throwExceptionOnNotFound)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            MethodInfo methodInfo = null;

            var methods = type.GetMethods(GetFlag(isPublic, isStatic));

            if (ignoreCase)
            {
                methodInfo = methods.FirstOrDefault(p => p.Name.ToUpperEN() == methodName.ToUpperEN());
            }
            else
            {
                methodInfo = methods.FirstOrDefault(p => p.Name == methodName);
            }

            if (methodInfo == null && throwExceptionOnNotFound)
            {
                throw new MissingMemberException(type.FullName + "::" + methodName);
            }

            return methodInfo;
        }

        /// <summary>
        ///     Gets the property.
        /// </summary>
        public static PropertyInfo GetProperty(this Type type, string propertyName, bool? isPublic, bool? isStatic, bool ignoreCase, bool throwExceptionOnNotFound)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyInfo propertyInfo = null;

            var fields = type.GetProperties(GetFlag(isPublic, isStatic));

            if (ignoreCase)
            {
                propertyInfo = fields.FirstOrDefault(p => p.Name.ToUpperEN() == propertyName.ToUpperEN());
            }
            else
            {
                propertyInfo = fields.FirstOrDefault(p => p.Name == propertyName);
            }

            if (propertyInfo == null && throwExceptionOnNotFound)
            {
                throw new MissingMemberException(type.FullName + "::" + propertyName);
            }

            return propertyInfo;
        }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                                 richTextBox.Document.ContentEnd).Text;
        }

        /// <summary>
        ///     Removes from end.
        /// </summary>
        public static string RemoveFromEnd(this string data, string value)
        {
            if (data.EndsWith(value, StringComparison.CurrentCulture))
            {
                return data.Substring(0, data.Length - value.Length);
            }

            return data;
        }

        /// <summary>
        ///     Removes from start.
        /// </summary>
        public static string RemoveFromStart(this string data, string value)
        {
            if (data == null)
            {
                return null;
            }

            if (data.StartsWith(value, StringComparison.CurrentCulture))
            {
                return data.Substring(value.Length, data.Length - value.Length);
            }

            return data;
        }

        /// <summary>
        ///     Sets the text.
        /// </summary>
        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        /// <summary>
        ///     To the upper en.
        /// </summary>
        public static string ToUpperEN(this string value)
        {
            return value.ToUpper(new CultureInfo("en-US"));
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Gets the flag.
        /// </summary>
        static BindingFlags GetFlag(bool? isPublic, bool? isStatic)
        {
            var flags = BindingFlags.Default;

            if (isPublic == null)
            {
                flags |= BindingFlags.Public | BindingFlags.NonPublic;
            }
            else if (isPublic == true)
            {
                flags |= BindingFlags.Public;
            }
            else
            {
                flags |= BindingFlags.NonPublic;
            }

            if (isStatic == null)
            {
                flags |= BindingFlags.Instance | BindingFlags.Static;
            }
            else if (isStatic == true)
            {
                flags |= BindingFlags.Static;
            }
            else
            {
                flags |= BindingFlags.Instance;
            }

            return flags;
        }
        #endregion
    }
}