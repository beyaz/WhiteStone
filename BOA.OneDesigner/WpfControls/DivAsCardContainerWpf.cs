using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class DivAsCardContainerWpf : Grid, IHostItem, IEventBusListener
    {
        #region Constructors
        public DivAsCardContainerWpf()
        {
            ResetBackground();

        }
        #endregion

        #region Public Properties
        public Host               Host                      { get; set; }
        public bool               IsEnteredDropLocationMode { get; set; }
        public DivAsCardContainer Model                     => (DivAsCardContainer) DataContext;
        #endregion

        #region Public Methods
        public BCardWpf BChildAt(int index)
        {
            return (BCardWpf)Children[index];
        }

        internal int BChildCount => Children.Count;
        #endregion

        #region Methods
        internal void EnterDropLocationMode()
        {
            if (!CanDrop(Host.SelectedElement))
            {
                return;
            }

            Background = Brushes.AntiqueWhite;

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
            ResetBackground();

            if (!CanDrop(Host.SelectedElement))
            {
                return;
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

            CardLayout.ApplyForCardsContainer(this);
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

                var cardWpf = Host.CreateBCardWpf(bCard);

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

        void Should_delete_and_refresh_if_my_child_selected()
        {
            var willBeDeleteElement = Children.ToArray().FirstOrDefault(x=>x==Host.SelectedElement);
            if (willBeDeleteElement == null)
            {
                return;
            }

            var bCard = ((BCardWpf) willBeDeleteElement).Model;

            bCard.RemoveFromParent();

            Refresh();
        }

        void OnDrop(DropLocation dropLocation)
        {
            UpdateModel(dropLocation.TargetLocationIndex);
            Refresh();
        }

        void ResetBackground()
        {
            Background = Brushes.WhiteSmoke;
        }

        void UpdateModel(int insertIndex)
        {
            var wpf = Host.SelectedElement as BCardWpf;
            if (wpf != null)
            {
                if (wpf.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BCard{TitleInfo = LabelInfoHelper.CreateNewLabelInfo("Card")});
                    return;
                }

                wpf.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, wpf.Model);

                return;
            }

            throw Error.InvalidOperation();
        }
        #endregion

       

        void Should_ExitDropLocationMode_when_any_component_selected()
        {
            ExitDropLocationMode();
        }

        void Should_refresh_when_any_my_child_component_moved_or_deleted()
        {
            if (!SelectedElementIsMyChild)
            {
                return;
            }

            Refresh();
        }

        bool SelectedElementIsMyChild => Children.Contains(Host.SelectedElement);

        void OnWideChanged()
        {
            if (!SelectedElementIsMyChild)
            {
                return;
            }

            CardLayout.ApplyForCardsContainer(this);
        }
        

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Should_refresh_when_any_my_child_component_moved_or_deleted);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnDragElementSelected, Should_ExitDropLocationMode_when_any_component_selected);
            // Host.EventBus.Subscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.Subscribe(EventBus.ComponentDeleted, Should_delete_and_refresh_if_my_child_selected);
            
            Host.EventBus.Subscribe(EventBus.WideChanged, OnWideChanged);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();


            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Should_refresh_when_any_my_child_component_moved_or_deleted);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnDragElementSelected, Should_ExitDropLocationMode_when_any_component_selected);
            // Host.EventBus.UnSubscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Should_delete_and_refresh_if_my_child_selected);
            
            Host.EventBus.UnSubscribe(EventBus.WideChanged, OnWideChanged);
        }
        #endregion
    }
}