using System;
using System.Windows;
using System.Windows.Controls;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     The WPF extra
    /// </summary>
    class WpfExtra
    {
        #region Public Methods
        public static bool Button_Text(Builder builder, UIElement element, Node node)
        {
            if (element is Button)
            {
                if (node.Name == "Text")
                {
                    node.Name = "Content";
                }
            }

            return false;
        }

        /// <summary>
        ///     Riches the text box create.
        /// </summary>
        public static UIElement RichTextBox_Create(Builder builder, Node node)
        {
            var viewNode = node.Properties["view"];

            if (viewNode.ValueAsStringToUpperInEnglish == "TEXTAREA")
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

        /// <summary>
        ///     Transforms the name of the view.
        /// </summary>
        public static void TransformViewName(Node root)
        {
            root.Visit(node =>
            {
                var ui = node.Properties?["UI"];
                if (ui != null)
                {
                    ui.Name = "view";
                }
            });
        }
        #endregion
    }
}