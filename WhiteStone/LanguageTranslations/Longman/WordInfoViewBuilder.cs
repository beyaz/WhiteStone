using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.Common.Helpers;

namespace BOA.LanguageTranslations.Longman
{
    /// <summary>
    ///     The word information view builder
    /// </summary>
    public class WordInfoViewBuilder
    {
        #region Fields
        /// <summary>
        ///     The word information
        /// </summary>
        readonly WordInfo _wordInfo;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="WordInfoViewBuilder" /> class.
        /// </summary>
        public WordInfoViewBuilder(WordInfo wordInfo)
        {
            _wordInfo = wordInfo;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Creates this instance.
        /// </summary>
        public StackPanel Create()
        {
            var sp = new StackPanel();
            foreach (var entry in _wordInfo.Dictentries)
            {
                sp.Children.Add(Render(entry));
            }


            return RenderInScrollViewer(sp);

        }
        #endregion


        static StackPanel RenderInScrollViewer(StackPanel sp)
        {
            return new StackPanel
            {
                Children =
                {
                    new ScrollViewer
                    {
                        VerticalScrollBarVisibility   = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        Content                       = sp
                    }
                }
            };
        }


        #region Methods
        /// <summary>
        ///     Renders the specified data.
        /// </summary>
        StackPanel Render(Entry data)
        {
            var sp = new StackPanel();

            if (data.Topics?.Count > 0)
            {
                sp.Children.Add(new TextBlock
                {
                    Text         = "Related Topics: " + string.Join(" , ", data.Topics),
                    TextWrapping = TextWrapping.WrapWithOverflow
                });
            }

            foreach (var usageInfo in data.Usages)
            {
                sp.Children.Add(Render(usageInfo));
            }

            return RenderInScrollViewer(sp); 
        }

        /// <summary>
        ///     Renders the specified data.
        /// </summary>
        StackPanel Render(UsageInfo data)
        {
            var sp = new StackPanel();

            var definitions = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            if (data.ShortDefinition.IsNullOrWhiteSpace() == false)
            {
                definitions.Children.Add(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new Label
                        {
                            Content    = data.ShortDefinition,
                            FontSize   = 18,
                            FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                            Foreground = Brushes.Red
                        },
                        new Label
                        {
                            Content    = $"({data.ShortDefinitionTR})",
                            FontSize   = 18,
                            FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                            Foreground = Brushes.Red
                        }
                    }
                });
            }

            if (data.FullDefinition.IsNullOrWhiteSpace() == false)
            {
                definitions.Children.Add(new StackPanel
                {
                    Children =
                    {
                        new Label
                        {
                            Content    = data.FullDefinition,
                            FontSize   = 16,
                            FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                            Foreground = Brushes.DarkGray
                        },
                        new Label
                        {
                            Content    = $"({data.FullDefinitionTR})",
                            FontSize   = 15,
                            FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                            Foreground = Brushes.DarkGray
                        }
                    }
                });
            }

            sp.Children.Add(definitions);

            foreach (var example in data.Examples.Take(3))
            {
                sp.Children.Add(Render(example));
            }

            return RenderInScrollViewer(sp);
        }

        /// <summary>
        ///     Renders the specified data.
        /// </summary>
        StackPanel Render(Example data)
        {
            return new StackPanel
            {
                Margin = new Thickness(10),
                Children =
                {
                    new Border
                    {
                        BorderBrush     = new SolidColorBrush( (Color)ColorConverter.ConvertFromString("#d9d9d9")),
                        BorderThickness = new Thickness(1),
                        Child = new StackPanel
                        {
                            Children =
                            {
                                new Label
                                {
                                    Content    = data.Text,
                                    FontSize   = 16,
                                    FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                                    Foreground = Brushes.Gray
                                },
                                new Label
                                {
                                    Content    = $"({data.TextTR})",
                                    FontSize   = 16,
                                    FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                                    Foreground = Brushes.Gray
                                }
                            }
                        }
                    }
                }
            };
        }
        #endregion
    }
}