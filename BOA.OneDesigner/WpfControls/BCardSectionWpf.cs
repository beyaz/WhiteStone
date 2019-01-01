using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class BCardSectionWpf : WrapPanel, IEventBusDisposable, IHostItem
    {
        #region Constructors
        public BCardSectionWpf()
        {
            EventBus2.Subscribe(EventBus2.OnDragStarted, EnterDropLocationMode);
            EventBus2.Subscribe(EventBus2.OnAfterDropOperation, Refresh);
            EventBus2.Subscribe(EventBus2.RefreshFromDataContext, Refresh);

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        public Host Host                      { get; set; }
        public bool IsEnteredDropLocationMode { get; set; }
        #endregion

        #region Properties
        BCardSection Data => (BCardSection) DataContext;
        #endregion

        #region Public Methods
        public void OnDrop(DropLocation dropLocation)
        {
            // Surface.ExitDropLocationMode();

            var insertIndex = dropLocation.TargetLocationIndex;

            var bInput = UIContext.DraggingElement as BCardWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                Data.InsertItem(insertIndex, bInput.Data);

                return;
            }

            throw Error.InvalidOperation();
        }

        public void UnSubscribeFromEventBus()
        {
            EventBus2.UnSubscribe(EventBus2.OnDragStarted, EnterDropLocationMode);
            EventBus2.UnSubscribe(EventBus2.OnAfterDropOperation, Refresh);
            EventBus2.UnSubscribe(EventBus2.RefreshFromDataContext, Refresh);
        }
        #endregion

        #region Methods
        static bool CanDrop(UIElement dragElement)
        {
            if (dragElement is BCardWpf)
            {
                return true;
            }

            return false;
        }

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
                    Host                = Host,
                    OnDropAction        = OnDrop,
                    TargetLocationIndex = i
                };
                Children.Add(dropLocation);

                Children.Add(control);
            }

            Children.Add(new DropLocation
            {
                Host                = Host,
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

        void Refresh()
        {
            IsEnteredDropLocationMode = false;

            Children.RemoveAll();

            if (Data == null)
            {
                return;
            }

            foreach (var bCard in Data.Items)
            {
                var uiElement = Host.Create<BCardWpf>(bCard);
                uiElement.Margin = new Thickness(10);

                DragHelper.MakeDraggable(uiElement);

                Children.Add(uiElement);
            }
        }
        #endregion
    }
}