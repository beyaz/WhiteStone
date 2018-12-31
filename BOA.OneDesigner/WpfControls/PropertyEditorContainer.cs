using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{


    class BDataGridEditor:StackPanel
    {
        public void AddColumn()
        {
            (this.DataContext as BDataGrid)?.Columns.Add(new BDataGridColumnInfo
            {
                Label = new LabelInfo
                {
                    IsFreeText    =  true,
                    FreeTextValue = "??"
                }
            });

            EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }


        public BDataGridEditor()
        {
            MinWidth = 200;
            MinHeight = 200;

            this.LoadJson(@"
{ 
	Childs:[  
		{ui:'RequestIntellisenseTextBox', Text:'{Binding DataSourceBindingPath}', Label:'Data Source Binding' },
        {ui:'Button', Text:'Add Column',Click:'AddColumn'},
        {ui:'Button', Text:'Remove Column'}
	]
}

");
        }
    }

    public sealed class PropertyEditorContainer : GroupBox
    {
        #region Constructors
        public PropertyEditorContainer()
        {
            Header = "Properties";

            EventBus.Subscribe(EventBus.OnDragElementSelected,Refresh);

        }
        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

        public void Refresh()
        {
            Content = null;

            if (UIContext.SelectedElement == null)
            {
                DataContext = null;
                return;
            }

            DataContext = ((FrameworkElement)UIContext.SelectedElement).DataContext;

            if (DataContext == null)
            {
                return;
            }
            

            var sp = new StackPanel
            {
                Margin = new Thickness(10),
                DataContext = DataContext,
                MinWidth = 200,
                MinHeight = 200
            };

            

            var bInput = DataContext as JsxElementModel.BInput;
            if (bInput != null)
            {
                sp.LoadJson(@"

{ 
	Childs:[  
		{ui:'RequestIntellisenseTextBox', Text:'{Binding BindingPath}', Label:'Binding Path' },
		{ui:'LabelEditor', MarginTop:10, DataContext:'{Binding LabelInfo}'}
	]
}

");
                Content = sp;
                return;
            }

            var bCard = DataContext as JsxElementModel.BCard;
            if (bCard != null)
            {
                sp.LoadJson(@"
{ 
	Childs:[  
		{ui:'LabelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding TitleInfo}'}
	]
}

");
                Content = sp;
                return;
            }


            var dataGridInfo = DataContext as JsxElementModel.BDataGrid;
            if (dataGridInfo != null)
            {
                var editor = new BDataGridEditor()
                {
                    DataContext = DataContext
                };
                sp.Children.Add(editor);

                Content = sp;
                return;
            }
          
            throw new ArgumentException();
        }

       
        
    }
}