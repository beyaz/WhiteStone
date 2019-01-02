using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel, IEventBusDisposable, IHostItem
    {
        #region Constructors
        public JsxElementDesignerSurface()
        {
            Background        = Brushes.WhiteSmoke;
            VerticalAlignment = VerticalAlignment.Stretch;

            EventBus2.Subscribe(EventBus2.OnDragStarted, EnterDropLocationMode);
            EventBus2.Subscribe(EventBus2.OnAfterDropOperation, ExitDropLocationMode);
            EventBus2.Subscribe(EventBus2.RefreshFromDataContext, Refresh);
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void UnSubscribeFromEventBus()
        {
            EventBus2.UnSubscribe(EventBus2.OnDragStarted, EnterDropLocationMode);
            EventBus2.UnSubscribe(EventBus2.OnAfterDropOperation, ExitDropLocationMode);
            EventBus2.UnSubscribe(EventBus2.RefreshFromDataContext, Refresh);
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
            var bInput = Host.DraggingElement as BCardWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                DataContext = new BCardSection
                {
                    Items = new List<BCard>
                    {
                        bInput.Data
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

            var cardSection = DataContext as BCardSection;

            if (cardSection != null)
            {
                var bCardSection = Host.Create<BCardSectionWpf>(cardSection);

                Children.Add(bCardSection);
                return;
            }

            throw Error.InvalidOperation();
        }
        #endregion
    }
}