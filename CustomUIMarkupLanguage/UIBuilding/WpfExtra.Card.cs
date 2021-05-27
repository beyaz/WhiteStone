using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    class Card : Border, IAddChild
    {
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
            Background   = WpfExtra.ToBrush("White");
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
}