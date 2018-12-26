using System;
using System.Windows;
using System.Windows.Controls;

namespace BOA.OneDesigner.WpfControls
{
    public class BCardSectionWpf : WrapPanel,  IJsxElementDesignerSurfaceItem
    {

        public BCardSectionWpf()
        {
            EventBus.DragStarted        += EnterDropLocationMode;
            EventBus.AfterDropOperation += ExitDropLocationMode;
        }
        #region Public Properties
        public bool                       IsEnteredDropLocationMode { get; set; }
        public JsxElementDesignerSurface Surface                   { get; set; }
        #endregion

        #region Properties
        JsxElementModel.BCardSection Data => (JsxElementModel.BCardSection) DataContext;
        #endregion

        #region Public Methods
         void EnterDropLocationMode()
        {
            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

            

            if (!CanDrop(UIContext.DraggingElement))
            {
                return;
            }

            var items = Children.ToArray();

            Children.Clear();

            for (var i = 0; i < items.Length; i++)
            {

                var control = items[i];
                var dropLocation = new DropLocation
                {
                    OnDropAction        = OnDrop,
                    TargetLocationIndex = i
                };
                Children.Add(dropLocation);


                Children.Add(control);
            }

            Children.Add(new DropLocation
            {
                OnDropAction        = OnDrop,
                TargetLocationIndex = items.Length
            });
        }

         void ExitDropLocationMode()
        {
            if (!(UIContext.DraggingElement is BCardWpf))
            {
                
            }

            if (!IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = false;

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
            // Surface.ExitDropLocationMode();

            var insertIndex = dropLocation.TargetLocationIndex;

            var bInput = UIContext.DraggingElement as BCardWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                Data.InsertItem(insertIndex, bInput.Data);

                RefreshDataContext();

                return;
            }

            throw new ArgumentException();
        }

        public void Refresh()
        {
            Children.Clear();

            if (Data == null)
            {
                return;
            }

            foreach (var bField in Data.Items)
            {
                var uiElement = new BCardWpf
                {
                    Surface     = Surface,
                    DataContext = bField,
                    Margin      = new Thickness(10),
                    // Container   = this
                };

                DragAndDropHelper.MakeDraggable(uiElement);

                Children.Add(uiElement);
            }
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

        static bool CanDrop(UIElement dragElement)
        {
            if (dragElement is BCardWpf)
            {
                return true;
            }

            return false;
        }

        void RefreshDataContext()
        {
            var dataContext = DataContext;
            DataContext = null;
            DataContext = dataContext;
        }
        #endregion
    }
}