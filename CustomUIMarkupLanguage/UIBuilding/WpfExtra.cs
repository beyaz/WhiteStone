using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using CustomUIMarkupLanguage.Markup;
using Newtonsoft.Json.Linq;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     The WPF extra
    /// </summary>
    class WpfExtra
    {
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

        public static void CardCreationEnd(Builder builder, UIElement element, Node node)
        {
            if (!(element is Card card))
            {
                return;
            }

            card.Child.LoadJson("{ spacing: 15 }");
        }

        public static UIElement CreateCard(Builder builder, Node node)
        {
            if (node.UI == nameof(Card).ToUpperEN())
            {
                return new Card();
            }

            return null;
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
        static Brush ToBrush(string hexaDecimalColorValue)
        {
            var color = ColorConverter.ConvertFromString(hexaDecimalColorValue);
            if (color == null)
            {
                throw new ArgumentException(hexaDecimalColorValue);
            }

            return new SolidColorBrush((Color) color);
        }
        #endregion

        class Card : Border, IAddChild
        {
            #region Constants
            const string UITemplate = @"
{
	Child:
	{
        view:'StackPanel',
        spacing: 15,
        Name:'" + nameof(panel) + @"',
        Children:
        [
            {view:'TextBlock', Text:'{Binding Header}' }    
        ]
	}	
}";
            #endregion

            #region Fields
            #pragma warning disable 649
            StackPanel panel;
            #pragma warning restore 649
            #endregion

            #region Constructors
            public Card()
            {
                CornerRadius = new CornerRadius(10);
                Background   = ToBrush("White");
                Effect = new DropShadowEffect
                {
                    BlurRadius  = 20,
                    Color       = Colors.DarkGray,
                    Opacity     = 0.4,
                    Direction   = 280,
                    ShadowDepth = 0
                };

                this.LoadJson(UITemplate);
            }
            #endregion

            #region Explicit Interface Methods
            void IAddChild.AddChild(object value)
            {
                if (ReferenceEquals(value, panel))
                {
                    Child = panel;
                    return;
                }

                panel.Children.Add((UIElement) value);
            }

            void IAddChild.AddText(string text)
            {
                throw new NotImplementedException(text);
            }
            #endregion

            #region string Header
            public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Card), new PropertyMetadata(default(string)));

            public string Header
            {
                get { return (string) GetValue(HeaderProperty); }
                set { SetValue(HeaderProperty, value); }
            }
            #endregion
        }

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