using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BComboBoxEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        public SizeEditor  _sizeEditor;
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

        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(BComboBox.IsVisibleBindingPath) + @"}', Label:'Is Visible' },
        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(BComboBox.IsDisabledBindingPath) + @"}', Label:'Is Disabled' },

        {ui:'RequestIntellisenseTextBox', SearchByCurrentSelectedDataGridDataSourceContract:true, Margin:5, Text:'{Binding " + nameof(BComboBox.ValueMemberPath) + @"}', Label:'Value Member Path' },
        {ui:'RequestIntellisenseTextBox', SearchByCurrentSelectedDataGridDataSourceContract:true, Margin:5, Text:'{Binding " + nameof(BComboBox.DisplayMemberPath) + @"}', Label:'Display Member Path' },

        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
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
        BInput Model => (BInput) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }
        #endregion
    }
}