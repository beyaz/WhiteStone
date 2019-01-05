using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel,  IHostItem
    {
        #region Constructors
        public JsxElementDesignerSurface()
        {
            Background        = Brushes.WhiteSmoke;
            VerticalAlignment = VerticalAlignment.Stretch;

            Loaded   += (s, e) => { AttachToEventBus(); };
            Unloaded += (s, e) => { DeAttachToEventBus(); };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.RefreshFromDataContext, Refresh);
        }

        public void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.RefreshFromDataContext, Refresh);
        }
        #endregion

        #region Methods
        void EnterDropLocationMode()
        {
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
            var cardWpf = Host.DraggingElement as BCardWpf;
            if (cardWpf != null)
            {
                cardWpf.Data.RemoveFromParent();

                DataContext = new DivAsCardContainer
                {
                    Items = new List<BCard>
                    {
                        cardWpf.Data
                    }
                };

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

            var cardSection = DataContext as DivAsCardContainer;

            if (cardSection != null)
            {
                var bCardSection = Host.Create<DivAsCardContainerWpf>(cardSection);

                Children.Add(bCardSection);
                return;
            }

            throw Error.InvalidOperation();
        }
        #endregion
    }
}