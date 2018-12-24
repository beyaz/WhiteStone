using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.DragAndDrop
{
    public class Info
    {
        #region Static Fields
        public static Info Current;
        #endregion

        #region Fields
        public Point StartPoint;
        #endregion

        #region Public Properties
        public UIElement Sender { get; set; }
        #endregion
    }

    public class JsxElementDesignerSurface : StackPanel,IDropLocationContainer
    {
        public JsxElementDesignerSurface()
        {
            
        }

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