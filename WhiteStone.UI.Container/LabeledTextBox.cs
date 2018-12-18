using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;

namespace WhiteStone.UI.Container
{
    public class LabeledTextBox : Grid
    {
        public static UIElement On(CustomUIMarkupLanguage.UIBuilding.Builder builder,Node node)
        {
            if (node.Properties["view"].ValueAsStringToUpperInEnglish == nameof(LabeledTextBox).ToUpperEN())
            {
                return new LabeledTextBox();
            }

            return null;
        }

        #region Constants
        const string UITemplate = @"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding Label}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Text:'{Binding Text}'}        
	]
	
}";
        #endregion

        #region Static Fields
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(default(string)));
        #endregion

        #region Constructors
        public LabeledTextBox()
        {
            var builder = new Builder
            {
                DataContext = this,
                Caller = this,
                
            };

            builder.Load(UITemplate);
        }
        #endregion

        #region Public Properties
        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion
    }
}