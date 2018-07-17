using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

            return sp;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Renders the specified data.
        /// </summary>
        StackPanel Render(Entry data)
        {
            var sp = new StackPanel();

            sp.Children.Add(new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        Content = "Related Topics: " + string.Join(" , ", data.Topics)
                    }
                }
            });

            foreach (var usageInfo in data.Usages)
            {
                sp.Children.Add(Render(usageInfo));
            }

            return sp;
        }

        /// <summary>
        ///     Renders the specified data.
        /// </summary>
        StackPanel Render(UsageInfo data)
        {
            var sp = new StackPanel();

            var definitions = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                
            };

            if (data.ShortDefinition.IsNullOrWhiteSpace() == false)
            {
                definitions.Children.Add(new Label
                {
                    Content    = data.ShortDefinition,
                    FontSize   = 18,
                    FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                    Foreground = Brushes.Red
                });
            }
            if (data.FullDefinition.IsNullOrWhiteSpace() == false)
            {
                definitions.Children.Add(new Label
                {
                    Content = " : ",
                    FontSize   = 16,
                    FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                    Foreground = Brushes.DarkGray
                });
                definitions.Children.Add(new Label
                {
                    Content    = data.FullDefinition,
                    FontSize   = 16,
                    FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                    Foreground = Brushes.DarkGray
                });
            }




            sp.Children.Add(definitions);

            foreach (var example in data.Examples)
            {
                sp.Children.Add(Render(example));
            }

            return sp;
        }

        /// <summary>
        ///     Renders the specified data.
        /// </summary>
        StackPanel Render(Example data)
        {
            var label = new Label
            {
                Content = data.Text,
                FontSize = 16,
                FontFamily = new FontFamily("arial, helvetica, sans-serif"),
                Foreground = Brushes.Gray
            };

            return new StackPanel
            {
                Children =
                {
                    label
                    
                }
            };
        }
        #endregion
    }
}