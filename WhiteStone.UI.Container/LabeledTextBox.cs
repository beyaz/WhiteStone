using System;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.UIBuilding;

namespace WhiteStone.UI.Container
{
    public class LabeledTextBox : Grid
    {
        #region Constants
        const string UITemplate = @"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding Label}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Text:'{Binding Text}' , Name:'" + nameof(_textBox) + @"' }        
	]
	
}";
        #endregion

        #region Static Fields
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(default(string)));
        #endregion

        #region Fields
        public TextBox _textBox;
        #endregion

        #region Constructors
        public LabeledTextBox()
        {
            DataContext = this;

            this.LoadJson(UITemplate);

            _textBox.TextChanged += (s, e) => { OnTextChanged(); };
        }
        #endregion

        #region Public Events
        public event Action TextChanged;
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

        #region Public Methods
        public static UIElement On(Builder builder, Node node)
        {
            if (node.UI == nameof(LabeledTextBox).ToUpperEN())
            {
                return new LabeledTextBox();
            }

            if (node.UI == nameof(TextBox).ToUpperEN() &&
                node.HasProperty("Label"))
            {
                return new LabeledTextBox();
            }

            return null;
        }
        #endregion

        #region Methods
        void OnTextChanged()
        {
            TextChanged?.Invoke();
        }
        #endregion
    }
}