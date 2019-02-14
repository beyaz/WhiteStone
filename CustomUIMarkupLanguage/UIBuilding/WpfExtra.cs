using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     The WPF extra
    /// </summary>
    class WpfExtra
    {

        class IsBoldConverter : IValueConverter
        {
            #region IValueConverter Members
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                {
                    return null;
                }


                if (System.Convert.ToBoolean(value))
                {
                    return FontWeights.Bold;
                }

                return FontWeights.Normal;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                {
                    return null;
                }

                if (value is FontWeight )
                {
                    return ((FontWeight) value) == FontWeights.Bold;
                }

                return false;
            }
            #endregion
        }


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
        } public static bool RadioButton_Label(Builder builder, UIElement element, Node node)
        {
            if (element is RadioButton)
            {
                if (node.NameToUpperInEnglish == "LABEL" || node.NameToUpperInEnglish == "TEXT")
                {
                    node.Name = "Content";
                }
            }

            return false;
        }

        
        public static bool IsVisible(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "ISVISIBLE")
            {
                if (node.ValueIsBoolean)
                {
                    if (node.ValueAsBoolean)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        element.Visibility = Visibility.Collapsed;
                    }

                    return true;

                }

                if (node.ValueIsBindingExpression)
                {
                    node.ValueAsBindingInfo.ConverterTypeFullName = typeof(BooleanToVisibilityConverter).FullName;

                    node.Name = "Visibility";
                }
            }

            return false;
        }

        public static bool VerticalAlignmentIsCenter(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "VERTICALALIGNISCENTER" ||
                node.NameToUpperInEnglish == "VALIGNISCENTER")
            {
                if (node.ValueIsBoolean)
                {
                    if (node.ValueAsBoolean)
                    {
                        ((FrameworkElement)element).VerticalAlignment = VerticalAlignment.Center;
                    }
                    else
                    {
                        ((FrameworkElement)element).VerticalAlignment = default(VerticalAlignment);
                    }

                    return true;

                }
            }

            return false;
        }
        public static bool HorizontalAlignmentIsCenter(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "HORIZONTALALIGNISCENTER" ||
                node.NameToUpperInEnglish == "HALIGNISCENTER")
            {
                if (node.ValueIsBoolean)
                {
                    if (node.ValueAsBoolean)
                    {
                        ((FrameworkElement)element).HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else
                    {
                        ((FrameworkElement)element).HorizontalAlignment = default(HorizontalAlignment);
                    }

                    return true;

                }
            }

            return false;
        }
        

        /// <summary>
        ///     Riches the text box create.
        /// </summary>
        public static UIElement RichTextBox_Create(Builder builder, Node node)
        {
            if (node.UI == "TEXTAREA")
            {
                return new RichTextBox
                {
                    AcceptsTab = true
                };
            }

            return null;
        }

        public static UIElement ListBox_Create(Builder builder, Node node)
        {
            if (node.UI == nameof(ListBox).ToUpperEN())
            {
                var ui = new ListBox();

                ui.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty,ScrollBarVisibility.Auto);

                return ui;
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

                    var propertyInfo = builder.Caller.GetType().GetProperty(node.ValueAsString,  isPublic: true,isStatic: false,ignoreCase: true,throwExceptionOnNotFound: true);

                    throw new Exception("TODO");
                    // propertyInfo.SetValue(builder.DataContext, text);
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

                if (node.ValueIsBindingExpression)
                {
                    node.ValueAsBindingInfo.ConverterTypeFullName = typeof(IsBoldConverter).FullName;

                    node.Name = nameof(TextBlock.FontWeight);
                    return false;
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