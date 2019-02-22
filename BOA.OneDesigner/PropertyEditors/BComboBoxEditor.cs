using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BComboBoxEditor : StackPanel, IHostItem
    {
        public void OnIsMultiSelectChanged()
        {
            
        }

        #region Fields
        public UIElement _valueChanged;
        public LabelEditor _labelEditor;
        public SizeEditor  _sizeEditor;
        public CheckBox _isMultiSelect;
        #endregion

        #region Constructors
        public BComboBoxEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Margin:5, Text:'{Binding " + nameof(BComboBox.SelectedValueBindingPath) + @"}', Label:'Selected Value Binding Path' },
		{ui:'LabelEditor', Name:'" + nameof(_labelEditor) + @"', DataContext:'{Binding " + nameof(BComboBox.LabelInfo) + @"}'},
        {ui:'SizeEditor',Name:'" + nameof(_sizeEditor) + @"',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(BComboBox.SizeInfo) + @"}'},

        

        {ui:'RequestIntellisenseTextBox', Margin:5, SearchByCurrentSelectedDataGridDataSourceContract:true,  Text:'{Binding " + nameof(BComboBox.ValueMemberPath) + @"}', Label:'Value Member Path' },
        {ui:'RequestIntellisenseTextBox', Margin:5, SearchByCurrentSelectedDataGridDataSourceContract:true,  Text:'{Binding " + nameof(BComboBox.DisplayMemberPath) + @"}', Label:'Display Member Path' }
        ,
        {   
            ui       :'CheckBox', 
            Content  :'Is Multi Select', 
            MarginTop: 10, 
            IsChecked: '{Binding " + nameof(Model.IsMultiSelect) + @"}', 
            Checked  : '" + nameof(OnIsMultiSelectChanged) + @"',
            Unchecked: '" + nameof(OnIsMultiSelectChanged) + @"',
            Name     : '" + nameof(_isMultiSelect)+@"'
        }
        ,
        {
            ui      : 'Expander',
            Header  : 'Visual',
            Content :            
            {
                ui:'StackPanel',
                Childs:
                [
                    {
                        ui      : 'RequestIntellisenseTextBox', 
                        Margin  : 5, 
                        ShowOnlyBooleanProperties:true, 
                        Text    : '{Binding " + nameof(BComboBox.IsVisibleBindingPath) + @"}', 
                        Label   : 'Is Visible' 
                    }
                    ,
                    {
                        ui      : 'RequestIntellisenseTextBox',
                        Margin  : 5, 
                        ShowOnlyBooleanProperties:true, 
                        Text    : '{Binding " + nameof(BComboBox.IsDisabledBindingPath) + @"}', 
                        Label   : 'Is Disabled' 
                    }
                ]
            }
            
        }
        ,
        {
            ui      : 'Expander',
            Header  : 'Events',
            Content :            
            {
                ui:'StackPanel',
                Childs:
                [   
                    {
                        ui                          : 'RequestIntellisenseTextBox', 
                        Margin                      : 5, 
                        ShowOnlyOrchestrationMethods: true, 
                        Name                        : '"+nameof(_valueChanged)+@"', 
                        Text                        : '{Binding " + nameof(Model.ValueChangedOrchestrationMethod) + @"}', 
                        Label                       : 'On Value Changed' 
                    }
                ]
            }
            
        }
        ,
        

        {ui:'Button', Text:'Delete', MarginTop: 20, Click:'" + nameof(Delete) + @"'}
	]
}

");

            Loaded += (s, e) =>
            {
                _labelEditor.Host = Host;
                _sizeEditor.Host  = Host;

            };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        BComboBox Model => (BComboBox) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }
        #endregion
    }
}