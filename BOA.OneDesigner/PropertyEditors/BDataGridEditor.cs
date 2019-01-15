using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BDataGridEditor : StackPanel, IHostItem
    {
        #region Fields
        public SizeEditor _sizeEditor;
        public Button _removeButton;
        #endregion

        #region Constructors
        public BDataGridEditor()
        {
            this.LoadJson(@"
{ 
  MinHeight:300,
   Margin:5,
	Childs:[  
		{ui:'RequestIntellisenseTextBox', ShowOnlyCollectionProperties:true, Text:'{Binding " + nameof(BDataGrid.DataSourceBindingPath) + @"}', Label:'Data Source Binding' },

        {
            ui:'Groupbox',Header:'Row Selection Changed',Margin:10,
            Content:
            {
                ui:'Grid',rows:[
                    {ui:'RequestIntellisenseTextBox', ShowOnlyOrchestrationMethods:true, Text:'{Binding " + nameof(BDataGrid.RowSelectionChangedOrchestrationMethod) + @"}', Label:'Orchestration' },    
                    {ui:'RequestIntellisenseTextBox', Text:'{Binding " + nameof(BDataGrid.SelectedRowDataBindingPath) + @"}', Label:'Binding Path' }
                ]
            }
        },

        {ui:'SizeEditor',Name:'" + nameof(_sizeEditor) + @"',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(BDataGrid.SizeInfo) + @"}'},

        {ui:'Button',Margin:10, Text:'Add Column',Click:'" + nameof(AddColumn) + @"'}
	]
}

");

            Loaded += (s, e) =>
            {
                if (Model.SizeInfo == null)
                {
                    Model.SizeInfo = new SizeInfo
                    {
                        IsMedium = true
                    };
                }
                _sizeEditor.Host  = Host;

            };

        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        BDataGrid Model => (BDataGrid) DataContext;
        #endregion

        #region Public Methods
        public void AddColumn()
        {
            Host.EventBus.Publish(EventBus.DataGridColumnAdded);
        }
        #endregion
    }
}