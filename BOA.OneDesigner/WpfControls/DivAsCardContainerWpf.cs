using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class DivAsCardContainerWpf : StackPanel, IHostItem
    {
        #region Constructors
        public DivAsCardContainerWpf()
        {
            Loaded   += (s, e) => { AttachToEventBus(); };
            Unloaded += (s, e) => { DeAttachToEventBus(); };
            Loaded   += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        public Host               Host                      { get; set; }
        public bool               IsEnteredDropLocationMode { get; set; }
        public DivAsCardContainer Model                     => (DivAsCardContainer) DataContext;
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

        void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.Subscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.Subscribe(EventBus.ComponentDeleted, Refresh);
        }

        void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.UnSubscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Refresh);
        }

        void EnterDropLocationMode()
        {
            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

            if (!CanDrop(Host.DraggingElement))
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
            if (!(Host.DraggingElement is BCardWpf))
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

        void OnDrop(DropLocation dropLocation)
        {
            // Surface.ExitDropLocationMode();

            var insertIndex = dropLocation.TargetLocationIndex;

            var bInput = Host.DraggingElement as BCardWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                Model.InsertItem(insertIndex, bInput.Data);

                return;
            }

            throw Error.InvalidOperation();
        }

        void Refresh()
        {
            IsEnteredDropLocationMode = false;

            Children.RemoveAll();

            if (Model == null)
            {
                return;
            }

            foreach (var bCard in Model.Items)
            {
                bCard.Container = Model;

                var cardWpf = Host.Create<BCardWpf>(bCard);

                cardWpf.Margin = new Thickness(10);

                Host.DragHelper.MakeDraggable(cardWpf);

                Children.Add(cardWpf);
            }
        }
        #endregion
    }
}