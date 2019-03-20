using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.UIBuilding;
using DotNetKit.Windows.Controls;

namespace WhiteStone.UI.Container
{
    public class LabeledComboBox : Grid
    {

        public event TextChangedEventHandler TextChanged
        {
            add { PART_AutoCompleteComboBox.TextChanged    += value; }
            remove { PART_AutoCompleteComboBox.TextChanged -= value; }
        }

        public event SelectionChangedEventHandler SelectionChanged
        {
            add{AddHandler(Selector.SelectionChangedEvent,value);}
            remove {RemoveHandler(Selector.SelectionChangedEvent,value); }
        }

        public AutoCompleteComboBox PART_AutoCompleteComboBox;

        #region Constants
        const string UITemplate = @"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding Label}', MarginBottom:5, IsBold:true},
        {view:'AutoCompleteComboBox', 

            SelectedValue       : '{Binding SelectedValue}',
            Text                : '{Binding Text}',
            Name                : 'PART_AutoCompleteComboBox',
            ItemsSource         : '{Binding ItemsSource}' ,  
            DisplayMemberPath   : '{Binding DisplayMemberPath}' , 
            SelectedValuePath   : '{Binding SelectedValuePath}'}
	]
	
}";
        #endregion

        #region Constructors
        public LabeledComboBox()
        {
            DataContext = this;

            this.LoadJson(UITemplate);
        }
        #endregion

        #region Public Methods
        public static UIElement On(Builder builder, Node node)
        {
            if (node.UI == nameof(LabeledComboBox).ToUpperEN())
            {
                return new LabeledComboBox();
            }

            if (node.UI == nameof(ComboBox).ToUpperEN() &&
                node.HasProperty("Label"))
            {
                return new LabeledComboBox();
            }


            if ( node.UI == "COMBO"&&
                node.HasProperty("Label"))
            {
                return new LabeledComboBox();
            }
            return null;
        }
        #endregion

        #region ItemsSource
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
                                                                                                    "ItemsSource", typeof(IEnumerable), typeof(LabeledComboBox), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region SelectedValuePath
        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
                                                                                                          "SelectedValuePath", typeof(string), typeof(LabeledComboBox), new PropertyMetadata(default(IEnumerable)));

        public string SelectedValuePath
        {
            get { return (string) GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }
        #endregion

        #region DisplayMemberPath
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
                                                                                                          "DisplayMemberPath", typeof(string), typeof(LabeledComboBox), new PropertyMetadata(default(IEnumerable)));

        public string DisplayMemberPath
        {
            get { return (string) GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion

        #region SelectedValue
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(string), typeof(LabeledComboBox), new PropertyMetadata(default(string)));

        public string SelectedValue
        {
            get { return (string) GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        #endregion

        #region SelectedValue
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(LabeledComboBox), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region Label
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledComboBox), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion
    }
}