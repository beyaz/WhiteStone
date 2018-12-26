using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel,IDropLocationContainer
    {

        public JsxElementDesignerSurface()
        {
            UIContext.Register(this);

            Background = Brushes.WhiteSmoke;

            MouseMove += JsxElementDesignerSurface_MouseEnter;
        }

        void JsxElementDesignerSurface_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var surfaceItem = UIContext.DraggingElement as IJsxElementDesignerSurfaceItem;
            if (surfaceItem != null && surfaceItem.Surface == null)
            {
                surfaceItem.Surface = this;
                EnterDropLocationMode();
            }
        }

        public Point DraggingElementStartPoint => UIContext.DraggingElementStartPoint;

        public UIElement DraggingElement => UIContext.DraggingElement;

      

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

            if (Children.Count == 0)
            {
                var dropLocation = new DropLocation
                {
                    OnDropAction = OnDrop, TargetLocationIndex = 0,
                    Width = 50,
                    Height = 50
                };

                Children.Add(dropLocation);
            }
            
        }

        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            ExitDropLocationMode();

            var bInput = DraggingElement as BCard;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();
                ((BCard) bInput.Container)?.RefreshDataContext();

                DataContext = new JsxElementModel.BCardSection
                {
                    Items = new List<JsxElementModel.BCard>
                    {
                        bInput.Data
                    }
                };

                this.RefreshDataContext();

                return;
            }

            throw new ArgumentException();
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