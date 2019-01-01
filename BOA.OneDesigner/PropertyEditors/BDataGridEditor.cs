using System;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    [Serializable]
    public class BDataGridEditorModel
    {
        #region Public Properties
        public BDataGrid           Info                        { get; set; }
        public bool                RemoveColumnButtonIsVisible { get; set; }
        public BDataGridColumnInfo SelectedColumn              { get; set; }
        #endregion
    }

    class BDataGridEditor : StackPanel
    {
        
        #region Fields
        public StackPanel _labelEditor;
        public ListBox    _listBox;
        public Button     _removeButton;
        #endregion

        #region Constructors
        public BDataGridEditor()
        {

            EventBus2.Subscribe(EventBus2.OnComponentPropertyChanged,RefreshListBoxItemsSource);

            this.LoadJson(@"
{ 
  MinHeight:300,
   Margin:5,
	Childs:[  
		{ui:'RequestIntellisenseTextBox', Text:'{Binding " + nameof(BDataGridEditorModel.Info.DataSourceBindingPath) + @"}', Label:'Data Source Binding' },
        {ui:'Button', Text:'Add Column',Click:'"+ nameof(AddColumn)+@"'},
        {ui:'Button', Name:'_removeButton', Text:'Remove Column',  IsVisible:'{Binding " + nameof(BDataGridEditorModel.RemoveColumnButtonIsVisible) + @",Mode:OneWay}' },
        {ui:'ListBox', Name:'_listBox', Height:300,
             ItemsSource:'{Binding " + Model.AccessPathOf(m=>m.Info.Columns) + @",Mode:OneWay}', 
             SelectionChanged:'" + nameof(SelectedColumnChanged) + @"', 
             DisplayMemberPath:'"+ nameof(BDataGridEditorModel.SelectedColumn.LabelText)+@"',
             SelectedValue:'{Binding "+ nameof(BDataGridEditorModel.SelectedColumn)+@"}'

        },
        
        { 
            ui:'StackPanel',
            Name:'_labelEditor',
            DataContext:'{Binding "+ nameof(BDataGridEditorModel.SelectedColumn)+@"}',
            IsVisible:'{Binding "+ nameof(BDataGridEditorModel.RemoveColumnButtonIsVisible)+@",Mode:OneWay}',
	        Childs:[
		        {ui:'RequestIntellisenseTextBox', Text:'{Binding "+ Model.AccessPathOf(m=>m.SelectedColumn.BindingPath)+@"}', Label:'Binding Path' },
		        {ui:'LabelEditor', MarginTop:10, DataContext:'{Binding "+ Model.AccessPathOf(m=>m.SelectedColumn.Label)+@"}'}
	        ]
        }
        
	]
}

");
        }
        #endregion


        void RefreshListBoxItemsSource()
        {
            _listBox.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }
        #region Public Properties
        public BDataGridEditorModel Model { get; set; }
        #endregion

        #region Public Methods
        public void AddColumn()
        {
            Model.Info.Columns.Add(new BDataGridColumnInfo
            {
                Label = new LabelInfo
                {
                    IsFreeText    = true,
                    FreeTextValue = "??"
                }
            });

            

            EventBus2.Publish(EventBus2.OnComponentPropertyChanged);
        }
        #endregion

        #region Methods
        void SelectedColumnChanged()
        {
            if (Model.SelectedColumn == null)
            {
                Model.RemoveColumnButtonIsVisible = false;
            }
            else
            {
                Model.RemoveColumnButtonIsVisible = true;
            }

            this.RefreshDataContext();

            _listBox.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            _removeButton.GetBindingExpression(VisibilityProperty)?.UpdateTarget();
            _labelEditor.GetBindingExpression(VisibilityProperty)?.UpdateTarget();
        }
        #endregion
    }
}