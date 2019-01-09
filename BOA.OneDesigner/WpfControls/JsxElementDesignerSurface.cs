using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel, IHostItem
    {
        #region Constructors
        public JsxElementDesignerSurface()
        {
            VerticalAlignment = VerticalAlignment.Stretch;

            Loaded   += (s, e) => { AttachToEventBus(); };
            Unloaded += (s, e) => { DeAttachToEventBus(); };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }

        public DivAsCardContainer Model => (DivAsCardContainer) DataContext;
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.Subscribe(EventBus.BeforeDragElementSelected, BeforeDragElementSelected);
        }

        public void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.UnSubscribe(EventBus.BeforeDragElementSelected, BeforeDragElementSelected);
        }
        #endregion

        #region Methods
        void BeforeDragElementSelected()
        {
            var column = Host.LastSelectedUIElement as BDataGridColumnWpf;

            Host.LastSelectedUIElement_as_DataGrid_DataSourceBindingPath = column?.BDataGridInfoWpf?.Model?.DataSourceBindingPath;
        }

        void EnterDropLocationMode()
        {
            if (!(Host.DraggingElement is BCardWpf))
            {
                return;
            }

            if (Children.Count == 0)
            {
                var dropLocation = new DropLocation
                {
                    Host                = Host,
                    OnDropAction        = OnDrop,
                    TargetLocationIndex = 0,
                    Width               = 50,
                    Height              = 50
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

        void OnDrop(DropLocation dropLocation)
        {
            var card = (Host.DraggingElement as BCardWpf)?.Model;
            if (card != null)
            {
                card.RemoveFromParent();
                card.Container = Model;

                DataContext = new DivAsCardContainer
                {
                    Items = new List<BCard>
                    {
                        card
                    }
                };

                Refresh();
                return;
            }

            throw Error.InvalidOperation();
        }

        void Refresh()
        {
            Children.RemoveAll();

            if (DataContext == null)
            {
                return;
            }

            var bCardSection = Host.Create<DivAsCardContainerWpf>(Model);

            Children.Add(bCardSection);
        }
        #endregion
    }
}