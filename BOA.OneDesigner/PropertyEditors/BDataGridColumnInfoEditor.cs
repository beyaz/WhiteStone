using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
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
		{ui:'RequestIntellisenseTextBox', Margin:10, SearchByCurrentSelectedDataGridDataSourceContract:true,  Text:'{Binding " + nameof(Model.BindingPath) + @"}', Label:'Binding Path' },
		{ui:'LabelEditor',                Margin:10, Name:'" + nameof(_labelEditor) + @"', DataContext:'{Binding " + nameof(Model.Label) + @"}'},
        {ui:'RequestIntellisenseTextBox', Margin:10, ShowOnlyBooleanProperties:true,  Text:'{Binding " + nameof(Model.IsVisibleBindingPath) + @"}', Label:'Is Visible' },
        {ui:'Button',                     Margin:10, Text:'Remove Column', Click:'" + nameof(RemoveColumn) + @"'},
        {   
            ui          : 'LabeledTextBox', 
            Label       : 'Width',
            Text        : '{Binding " + Model.AccessPathOf(m => m.Width) + @", Converter = WhiteStone.UI.Container.StringToNullableInt32Converter}'
        },
        {   
            ui          : 'LabeledTextBox', 
            Label       : 'Date Format (Default: L)',
            Text        : '{Binding " + Model.AccessPathOf(m => m.DateFormat) + @"}'
        }
        ,
        {   
            ui          : 'LabeledTextBox', 
            Label       : 'Total Summary (Örnek: sum, count, avg)',
            Text        : '{Binding " + Model.AccessPathOf(m => m.TotalSummary) + @"}'
        }
	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion
        
        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        BDataGridColumnInfo Model => (BDataGridColumnInfo) DataContext;
        #endregion

        #region Public Methods
        public void RemoveColumn()
        {
            Host.EventBus.Publish(EventBus.DataGridColumnRemoved);
        }
        #endregion
    }
}