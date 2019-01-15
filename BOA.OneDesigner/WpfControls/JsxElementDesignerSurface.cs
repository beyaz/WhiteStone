using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel, IHostItem,IEventBusListener
    {


        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();
            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.Subscribe(EventBus.BeforeDragElementSelected, BeforeDragElementSelected);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();
            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.RefreshFromDataContext, Refresh);
            Host.EventBus.UnSubscribe(EventBus.BeforeDragElementSelected, BeforeDragElementSelected);
        }
        #endregion



        #region Constructors
        public JsxElementDesignerSurface()
        {
            VerticalAlignment = VerticalAlignment.Stretch;

            
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }

        public DivAsCardContainer Model => (DivAsCardContainer) DataContext;
        #endregion

        #region Public Methods
       

       
        #endregion

        #region Methods
        void BeforeDragElementSelected()
        {
            var column = Host.LastSelectedUIElement as BDataGridColumnWpf;

            Host.LastSelectedUIElement_as_DataGrid_DataSourceBindingPath = column?.BDataGridInfoWpf?.Model?.DataSourceBindingPath;

            var bComboBoxInWpf = Host.LastSelectedUIElement  as BComboBoxInWpf;
            if (bComboBoxInWpf != null)
            {
                Host.LastSelectedUIElement_as_DataGrid_DataSourceBindingPath = bComboBoxInWpf.Model.DataGrid.DataSourceBindingPath;
            }
        }

        void EnterDropLocationMode()
        {
            if (!(Host.SelectedElement is BCardWpf))
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
            var card = (Host.SelectedElement as BCardWpf)?.Model;
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
            Host.DeAttachToEventBus(Children);

            Children.Clear();

            if (DataContext == null)
            {
                return;
            }

            var bCardSection = Host.CreateDivAsCardContainerWpf(Model);

            Children.Add(bCardSection);

            Host.AttachToEventBus(bCardSection,this);
        }
        #endregion
    }
}