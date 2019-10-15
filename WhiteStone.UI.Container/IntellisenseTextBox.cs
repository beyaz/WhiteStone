using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.UIBuilding;
using DotNetKit.Windows.Controls;
using FeserWard.Controls;
using ReflectionHelper = CustomUIMarkupLanguage.ReflectionHelper;

namespace WhiteStone.UI.Container
{
    /// <summary>
    /// The intellisense text box
    /// </summary>
    public class IntellisenseTextBox : Grid
    {
        public AutoCompleteComboBox AutoCompleteComboBox;

        #region Constants
        const string UITemplate = @"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding Label}', MarginBottom:5, IsBold:true},
        {
            view:'AutoCompleteComboBox',
            SelectedValue:'{Binding Text}',
            Name: 'AutoCompleteComboBox'
        }
	]
	
}";
        #endregion

        

        #region Constructors
        public IntellisenseTextBox()
        {
            DataContext = this;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Builder.RegisterElementCreation(On);
            }

            this.LoadJson(UITemplate);

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Builder.RegisterElementCreation(On);
                return;
            }

           
        }
        #endregion

      

        #region Public Methods
        public static UIElement On(Builder builder, Node node)
        {
            if (node.UI == nameof(Intellibox).ToUpperEN())
            {
                return new Intellibox();
            }

            if (node.UI == nameof(TextBox).ToUpperEN() &&
                node.HasProperty("IntellisenseSource"))
            {
                
                return new IntellisenseTextBox();
            }

            return null;
        }
        #endregion

        

        #region Label
        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string)));
        #endregion

        #region IntellisenseSource

        public static readonly DependencyProperty IntellisenseSourceProperty = DependencyProperty.Register("IntellisenseSource", typeof(IReadOnlyList<string>), typeof(IntellisenseTextBox), new PropertyMetadata(default(IReadOnlyList<string>), (o, args) =>
        {
            ((IntellisenseTextBox) o).AutoCompleteComboBox.ItemsSource = (IEnumerable)args.NewValue;
        }));

        public IReadOnlyList<string> IntellisenseSource
        {
            get { return (IReadOnlyList<string>)GetValue(IntellisenseSourceProperty); }
            set { SetValue(IntellisenseSourceProperty, value); }
        } 
        #endregion


        #region IntellisenseSource

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string), (o, args) =>
        {
            ((IntellisenseTextBox) o).AutoCompleteComboBox.Text = (string)args.NewValue;
        }));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        } 
        #endregion


        public event SelectionChangedEventHandler TextChanged
        {
            add { AutoCompleteComboBox.SelectionChanged += value; }
            remove { AutoCompleteComboBox.SelectionChanged -= value; }
        }

    }
}