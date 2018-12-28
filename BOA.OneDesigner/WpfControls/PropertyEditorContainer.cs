using System;
using System.Windows;
using System.Windows.Controls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class PropertyEditorContainer : GroupBox
    {
        #region Constructors
        public PropertyEditorContainer()
        {
            Header = "Properties";

            EventBus.DragElementSelected += Refresh;

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
                DataContext = DataContext
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
                sp.LoadJson("{ Childs:[  {ui:'TextBox',Text:'{Binding "+nameof(bCard.Title)+"}' , Label:'Title' }  ] }");
                Content = sp;
                return;
            }

          
            throw new ArgumentException();
        }



        
    }
}