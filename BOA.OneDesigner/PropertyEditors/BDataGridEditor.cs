using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BDataGridEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        public Button      _removeButton;
        public GroupBox    _rowSelectionChangedGroupBox;
        public SizeEditor  _sizeEditor;

        public ActionInfoEditor _rowSelectionChangedActionInfoEditor;

        #endregion

        public static BDataGridEditor Create(Host host, BDataGrid info)
        {
            var componentEditor = new BDataGridEditor
            {
                Host        = host,
                DataContext = info
            };
            componentEditor.LoadUI();

            return componentEditor;
        }
        #region Constructors

        void LoadUI()
        {
            this.LoadJson(@"
{ 
  MinHeight:300,
   Margin:5,
	Childs:
    [  
		{
            ui:'RequestIntellisenseTextBox', 
            ShowOnlyCollectionProperties:true, 
            Text:'{Binding " + nameof(BDataGrid.DataSourceBindingPath) + @"}', 
            Label:'Data Source Binding' 
        }
        ,
        {
            ui:'Groupbox',
            Header:'Row Selection Changed',
            Margin:10,
            Name:'_rowSelectionChangedGroupBox',
            Content:
            {
                ui:'Grid',
                rows:
                [
                    {
                        ui:'RequestIntellisenseTextBox', 
                        Text:'{Binding " + nameof(BDataGrid.SelectedRowDataBindingPath) + @"}', 
                        Label:'Binding Path' 
                    }
                    ,
                    {
                        ui          : 'ActionInfoEditor',
                        Header      : 'On Row Selection Changed',
                        Name        : '"+nameof(_rowSelectionChangedActionInfoEditor)+@"',
                        MarginTop   : 10
                    }
                ]
            }
        },

        {ui:'SizeEditor',Name:'" + nameof(_sizeEditor) + @"',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(BDataGrid.SizeInfo) + @"}'},

        {ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(Model.TitleInfo) + @"}'},

        {ui:'Button',Margin:10, Text:'Add Column',Click:'" + nameof(AddColumn) + @"'},
        {ui:'Button',Margin:10, Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");


            if (Model.SizeInfo == null)
            {
                Model.SizeInfo = new SizeInfo
                {
                    IsMedium = true
                };
            }

            _sizeEditor.Host  = Host;
            _labelEditor.Host = Host;

            if (Model?.ParentIsComboBox == true)
            {
                _sizeEditor.Visibility                  = Visibility.Collapsed;
                _rowSelectionChangedGroupBox.Visibility = Visibility.Collapsed;
            }

            if (Model.RowSelectionChangedActionInfo == null)
            {
                Model.RowSelectionChangedActionInfo = new ActionInfo();
            }
            _rowSelectionChangedActionInfoEditor.Load(Host,Model.RowSelectionChangedActionInfo);

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

        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }
        #endregion
    }
}