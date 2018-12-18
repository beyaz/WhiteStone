using System;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.Jaml.Markup;

namespace BOA.Jaml
{
    /// <summary>
    ///     The WPF extra
    /// </summary>
    public class WpfExtra
    {
        #region Public Methods
        /// <summary>
        ///     Riches the text box create.
        /// </summary>
        public static UIElement RichTextBox_Create(Builder builder, Node node)
        {
            if (node.Properties["view"].ValueAsStringToUpperInEnglish == "TEXTAREA")
            {
                return new RichTextBox
                {
                    AcceptsTab = true
                };
            }

            return null;
        }

        /// <summary>
        ///     Riches the text box text.
        /// </summary>
        public static bool RichTextBox_Text(Builder builder, UIElement element, Node node)
        {
            var richTextBox = element as RichTextBox;
            if (richTextBox == null)
            {
                return false;
            }

            if (node.Name == "Text")
            {
                if (node.ValueIsString)
                {
                    richTextBox.SetText(node.ValueAsString);
                    return true;
                }

                richTextBox.TextChanged += (s, e) =>
                {
                    var text = richTextBox.GetText();

                    var propertyInfo = builder.DataContext.GetType().GetPublicNonStaticProperty(node.ValueAsString, true);

                    propertyInfo.SetValue(builder.DataContext, text);
                };

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Texts the block is bold.
        /// </summary>
        public static bool TextBlock_IsBold(Builder builder, UIElement element, Node node)
        {
            var textBlock = element as TextBlock;
            if (textBlock == null)
            {
                return false;
            }

            if (node.NameToUpperInEnglish == "ISBOLD")
            {
                if (node.ValueAsBoolean)
                {
                    textBlock.FontWeight = FontWeights.Bold;
                    return true;
                }

                throw new ArgumentException(node.ToString());
            }

            return false;
        }
        #endregion
    }
}