using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.DragAndDrop
{
   

    public interface IJsxElementDesignerSurface
    {
         Point DraggingElementStartPoint { get; set; }

         UIElement DraggingElement { get; set; }

         void EnterDropLocationMode();

         void ExitDropLocationMode();
    }

    public interface IJsxElementDesignerSurfaceItem
    {
        IJsxElementDesignerSurface Surface { get; set; }
    }

    public class JsxElementDesignerSurface : StackPanel,IDropLocationContainer,IJsxElementDesignerSurface
    {

        public Point DraggingElementStartPoint { get; set; }

        public UIElement DraggingElement { get; set; }

      

        public void Refresh()
        {
            Children.Clear();

            if (DataContext == null)
            {
                return;
            }

            var cardSection = DataContext as BCardSection;

            if (cardSection!= null)
            {
                var bCardSection = new WpfControls.BCardSection
                {
                    Surface = this,
                    DataContext = cardSection
                    
                };

                Children.Add(bCardSection);
                return;
            }

            throw new ArgumentException();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

        public void EnterDropLocationMode()
        {
            foreach (var child in Children)
            {
                (child as IDropLocationContainer)?.EnterDropLocationMode();
            }
        }

        public void ExitDropLocationMode()
        {
            foreach (var child in Children)
            {
                (child as IDropLocationContainer)?.ExitDropLocationMode();
            }
        }
    }


}