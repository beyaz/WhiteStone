using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.Jaml;

namespace WhiteStone.UI.Container
{
    public class LabeledTextBox : UserControl
    {
        public static void On(Builder builder)
        {
            if (builder.ViewName.ToUpperEN()== nameof(LabeledTextBox).ToUpperEN())
            {
                builder.View = new LabeledTextBox();
            }
        }

        #region Constants
        const string UITemplate = @"
{
    view:'Grid',
	rows:
	[
		{view:'TextBlock', Height:'auto', Text:'{Binding Label}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Height:'auto', Text:'{Binding Text}'}        
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
                DataContext = this
            };

            builder.SetJson(UITemplate).Build();

            Content = builder.View;
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