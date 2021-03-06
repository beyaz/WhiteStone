﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{

    public class LabeledTextBox : Grid
    {
        #region Constants
        const string UITemplate = @"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding Label}', IsBold:true, foreground: '#596B75' },
        {   
            ui: 'Border', 
            cornerRadius: 3, 
            padding: 1, 
            marginTop:5, 
            border: '0.2px solid Black',
            child: { ui:'TextBox', Text:'{Binding Text}', Name:'" + nameof(_textBox) + @"', borderThickness: 0 }        
        }
        
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