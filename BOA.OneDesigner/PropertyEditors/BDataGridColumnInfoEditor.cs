using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BDataGridColumnInfoEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        #endregion

        #region Constructors
        public BDataGridColumnInfoEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Margin:5, Text:'{Binding " + nameof(BDataGridColumnInfo.BindingPath) + @"}', Label:'Binding Path' },
		{ui:'LabelEditor', Name:'" + nameof(_labelEditor) + @"', DataContext:'{Binding " + nameof(BDataGridColumnInfo.Label) + @"}'},
        {ui:'Button', Text:'Remove Column',Click:'" + nameof(RemoveColumn) + @"'}
	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void RemoveColumn()
        {
            var selectedUI = (BDataGridColumnWpf) Host.LastSelectedUIElement;

            var dataGridInfoWpf = selectedUI.BDataGridInfoWpf;

            dataGridInfoWpf.BData.Columns.Remove(selectedUI.BData);

            Host.EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }
        #endregion
    }
}