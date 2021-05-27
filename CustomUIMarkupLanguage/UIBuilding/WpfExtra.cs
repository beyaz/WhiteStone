using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using CustomUIMarkupLanguage.Markup;
using Newtonsoft.Json.Linq;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     The WPF extra
    /// </summary>
    class WpfExtra
    {
        
        /// <summary>
        ///     Updates the UI after sleep.
        /// </summary>
        public static void UpdateUiAfterSleep(Dispatcher dispatcher, int sleepMilliseconds, Action action)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await dispatcher.BeginInvoke(action);
            });
        }


        #region Static Fields
        static readonly string[] MarginInChildrenNames =
        {
            "SPACING"
        };
        #endregion

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

       

        public static bool HasSimpleDropShadowEffect(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "HASSIMPLEDROPSHADOWEFFECT")
            {
                if (node.ValueIsBoolean)
                {
                    if (node.ValueAsBoolean)
                    {
                        element.Effect = new DropShadowEffect
                        {
                            BlurRadius  = 20,
                            Color       = Colors.DarkGray,
                            Opacity     = 0.4,
                            Direction   = 280,
                            ShadowDepth = 0
                        };
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
                        ((FrameworkElement) element).HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else
                    {
                        ((FrameworkElement) element).HorizontalAlignment = default(HorizontalAlignment);
                    }

                    return true;
                }
            }

            return false;
        }

        
        public static bool BorderStyle(Builder builder, UIElement element, Node node)
        {
            if (!(element is Border border ))
            {
                return false;
            }

            if (node.NameToUpperInEnglish == "BORDER")
            {
                var list = node.ValueAsString.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (list.Count == 3)
                {
                    border.BorderThickness = new Thickness(double.Parse(list[0].RemoveFromEnd("px")));
                    border.BorderBrush     = ToBrush(list[2]);

                    return true;
                }

                throw new NotImplementedException(node.ValueAsString);
            }

            return false;
        }

        
        
        public static bool ControlStyles(Builder builder, UIElement element, Node node)
        {
            if (!(element is Border control ))
            {
                return false;
            }

            if ("borderThickness".Equals( node.Name,StringComparison.OrdinalIgnoreCase))
            {
                control.BorderThickness = new Thickness(node.ValueAsNumberAsDouble);

                return true;
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

        public static UIElement ListBox_Create(Builder builder, Node node)
        {
            if (node.UI == nameof(ListBox).ToUpperEN())
            {
                var ui = new ListBox();

                ui.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);

                return ui;
            }

            return null;
        }

        
        public static UIElement StackPanelCreations(Builder builder, Node node)
        {
            if ("hStack".Equals(node.UI,StringComparison.OrdinalIgnoreCase))
            {
                var ui = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                return ui;
            }

            if ("vStack".Equals(node.UI,StringComparison.OrdinalIgnoreCase))
            {
                var ui = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                return ui;
            }

            return null;
        }


        public static bool MarginInChildren(Builder builder, UIElement element, Node node)
        {
            if (MarginInChildrenNames.Contains(node.NameToUpperInEnglish))
            {
                return true;
            }

            return false;
        }

        public static void MarginInChildrenEnd(Builder builder, UIElement element, Node node)
        {
            node = node.Properties[MarginInChildrenNames];

            if (node == null)
            {
                return;
            }

            var margin = Convert.ToDouble(node.ValueAsNumber);

            if (element is StackPanel stackPanel)
            {
                var isFirst = true;

                if (stackPanel.Orientation == Orientation.Vertical)
                {
                    foreach (FrameworkElement child in stackPanel.Children)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            continue;
                        }

                        child.Margin = new Thickness(child.Margin.Left, margin, child.Margin.Right, child.Margin.Bottom);
                    }
                }
                else
                {
                    foreach (FrameworkElement child in stackPanel.Children)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            continue;
                        }

                        child.Margin = new Thickness(margin, child.Margin.Top, child.Margin.Right, child.Margin.Bottom);
                    }
                }

                return;
            }

            throw new ArgumentException(node.ToString());
        }

        public static bool RadioButton_Label(Builder builder, UIElement element, Node node)
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

                    var propertyInfo = builder.Caller.GetType().GetProperty(node.ValueAsString, isPublic: true, isStatic: false, ignoreCase: true, throwExceptionOnNotFound: true);

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

        public static JToken TransformTitleInStackPanel(JToken jToken)
        {
            var jObject = jToken as JObject;

            if (jObject == null)
            {
                return jToken;
            }

            var value = jObject.GetValue("ui", StringComparison.OrdinalIgnoreCase) as JValue;

            if (value?.ToString().Equals("StackPanel", StringComparison.OrdinalIgnoreCase) == true ||
                value?.ToString().Equals("Grid", StringComparison.OrdinalIgnoreCase) == true)
            {
                var jValue = jObject.GetValue("title", StringComparison.OrdinalIgnoreCase) as JValue;
                if (jValue?.Value is string)
                {
                    jObject.Remove(jValue.Path);

                    var groupBox = JObject.Parse("{ui:'GroupBox'}");

                    groupBox.Add("Header", jValue);

                    groupBox.Add("Content", jObject);

                    return groupBox;
                }
            }

            return jObject;
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

        public static bool VerticalAlignmentIsCenter(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "VERTICALALIGNISCENTER" ||
                node.NameToUpperInEnglish == "VALIGNISCENTER")
            {
                if (node.ValueIsBoolean)
                {
                    if (node.ValueAsBoolean)
                    {
                        ((FrameworkElement) element).VerticalAlignment = VerticalAlignment.Center;
                    }
                    else
                    {
                        ((FrameworkElement) element).VerticalAlignment = default(VerticalAlignment);
                    }

                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Methods
        public static Brush ToBrush(string hexaDecimalColorValue)
        {
            var color = ColorConverter.ConvertFromString(hexaDecimalColorValue);
            if (color == null)
            {
                throw new ArgumentException(hexaDecimalColorValue);
            }

            return new SolidColorBrush((Color) color);
        }
        #endregion

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

                if (value is FontWeight)
                {
                    return (FontWeight) value == FontWeights.Bold;
                }

                return false;
            }
            #endregion
        }
    }
}