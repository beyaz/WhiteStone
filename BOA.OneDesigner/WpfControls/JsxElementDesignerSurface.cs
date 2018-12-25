using System;
using System.Windows;
using System.Windows.Controls;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel,IDropLocationContainer
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

            var cardSection = DataContext as JsxElementModel.BCardSection;

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