using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class DivAsCardContainerWpf : Grid, IHostItem, IEventBusListener
    {
        #region Constructors
        public DivAsCardContainerWpf()
        {
            ResetBackground();

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        public Host               Host                      { get; set; }
        public bool               IsEnteredDropLocationMode { get; set; }
        public DivAsCardContainer Model                     => (DivAsCardContainer) DataContext;
        #endregion

        #region Methods
        internal void EnterDropLocationMode()
        {
            Background = Brushes.AntiqueWhite;

            if (!CanDrop(Host.DraggingElement))
            {
                return;
            }

            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

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

            var lastDropLocation = new DropLocation
            {
                Host                = Host,
                OnDropAction        = OnDrop,
                TargetLocationIndex = items.Length
            };
            Children.Add(lastDropLocation);

            CardLayout.ApplyWithDropLocationMode(this);
        }

        internal void ExitDropLocationMode()
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

        internal void Refresh()
        {
            ResetBackground();

            IsEnteredDropLocationMode = false;

            Host.DeAttachToEventBus(Children);

            Children.Clear();

            if (Model == null)
            {
                return;
            }

            foreach (var bCard in Model.Items)
            {
                bCard.Container = Model;
                if (bCard.LayoutProps == null)
                {
                    bCard.LayoutProps = new LayoutProps();
                }

                var cardWpf = Host.Create<BCardWpf>(bCard);

                cardWpf.Margin = new Thickness(10);

                Host.DragHelper.MakeDraggable(cardWpf);

                Host.AttachToEventBus(cardWpf, this);

                Children.Add(cardWpf);
            }

            CardLayout.ApplyForCardsContainer(this);
        }

        static bool CanDrop(UIElement dragElement)
        {
            if (dragElement is BCardWpf)
            {
                return true;
            }

            return false;
        }

        void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var bCardWpf = Host.DraggingElement as BCardWpf;
            if (bCardWpf != null)
            {
                if (bCardWpf.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BCard());
                    return;
                }

                bCardWpf.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, bCardWpf.Model);

                return;
            }

            throw Error.InvalidOperation();
        }

        void ResetBackground()
        {
            Background = Brushes.WhiteSmoke;
        }
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.Subscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.Subscribe(EventBus.ComponentDeleted, Refresh);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.UnSubscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Refresh);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, Refresh);
        }
        #endregion
    }
}