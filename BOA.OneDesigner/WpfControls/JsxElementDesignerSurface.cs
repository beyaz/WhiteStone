using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel
    {
        #region Constructors
        public JsxElementDesignerSurface()
        {
            Background = Brushes.WhiteSmoke;

            EventBus.DragStarted += EnterDropLocationMode;
            EventBus.AfterDropOperation += ExitDropLocationMode;

            MouseMove += JsxElementDesignerSurface_MouseEnter;
        }
        #endregion

        #region Public Methods
        public void EnterDropLocationMode()
        {
            if (Children.Count == 0)
            {
                var dropLocation = new DropLocation
                {
                    OnDropAction = OnDrop, TargetLocationIndex = 0,
                    Width        = 50,
                    Height       = 50
                };

                Children.Add(dropLocation);
            }
        }

        void ExitDropLocationMode()
        {

            var items = Children.ToArray();

            Children.Clear();

            foreach (var control in items)
            {
                if (control is DropLocation)
                {
                    continue;
                }

                

                Children.Add(control);
            }
        }

        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            // ExitDropLocationMode();

            var bInput = UIContext.DraggingElement as BCardWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();
                ((BCardWpf) bInput.Container)?.RefreshDataContext();

                DataContext = new BCardSection
                {
                    Items = new List<BCard>
                    {
                        bInput.Data
                    }
                };

                this.RefreshDataContext();

                return;
            }

            throw new ArgumentException();
        }

        public void Refresh()
        {
            Children.Clear();

            if (DataContext == null)
            {
                return;
            }

            var cardSection = DataContext as BCardSection;

            if (cardSection != null)
            {
                var bCardSection = new BCardSectionWpf
                {
                    Surface     = this,
                    DataContext = cardSection
                };

                Children.Add(bCardSection);
                return;
            }

            throw new ArgumentException();
        }
        #endregion

        #region Methods
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

        void JsxElementDesignerSurface_MouseEnter(object sender, MouseEventArgs e)
        {
            var surfaceItem = UIContext.DraggingElement as IJsxElementDesignerSurfaceItem;
            if (surfaceItem != null && surfaceItem.Surface == null)
            {
                surfaceItem.Surface = this;
                EventBus.OnDragStarted();
                // EnterDropLocationMode();
            }
        }
        #endregion
    }
}