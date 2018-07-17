using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

            sp.Children.Add(new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        Content = data.ShortDefinition
                    },
                    new Label
                    {
                        Content = " : "
                    },
                    new Label
                    {
                        Content = data.FullDefinition
                    }
                }
            });

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
            var imagePath = @"d:\temp\Play.png";
            var image = new Image
            {
                Source = new BitmapImage(new Uri(imagePath))
            };

            return new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    image,
                    new Label
                    {
                        Content = data.Text
                    }
                }
            };
        }
        #endregion
    }
}