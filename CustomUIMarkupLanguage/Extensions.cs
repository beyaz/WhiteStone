using System;
using System.Globalization;
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
        ///     Does the cast operations on parameters for invoke method.
        /// </summary>
        public static void DoCastOperationsOnParametersForInvokeMethod(this MethodInfo mi, object[] parameters)
        {
            var parameterInfos = mi.GetParameters();

            if (parameterInfos.Length != parameters.Length)
            {
                throw new InvalidOperationException();
            }

            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];

                parameters[i] = Cast.To(parameters[i], parameterInfo.ParameterType, CultureInfo.CurrentCulture);
            }
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
        #endregion
    }
}