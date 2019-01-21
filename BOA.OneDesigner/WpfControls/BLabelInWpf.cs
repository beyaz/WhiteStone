using System;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    class BLabelInWpf : TextBlock, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        #region Public Properties
        public Host Host { get; set; }

        public BLabel Model => (BLabel) DataContext;

        public SizeInfo SizeInfo => Model.SizeInfo;
        #endregion

        #region Methods
        void UpdateLabel()
        {
            if (Host.SelectedElement != this)
            {
                return;
            }

            GetBindingExpression(TextProperty)?.UpdateTarget();
        }
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
        }
        #endregion
    }
}