using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    class BLabelInWpf : TextBlock, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        #region Constructors
        public BLabelInWpf()
        {
            Loaded += (s, e) => { UpdateTextProperty(); };
        }
        #endregion

        #region Public Properties
        public Host Host        { get; set; }
        public bool IsInToolbox { get; set; }

        public BLabel Model => (BLabel) DataContext;

        public SizeInfo SizeInfo => Model.SizeInfo;
        #endregion

        #region Methods
        void UpdateFontWeight()
        {
            if (Model.IsBold)
            {
                FontWeight = FontWeights.Bold;
            }
            else
            {
                FontWeight = FontWeights.Normal;
            }
        }

        void UpdateLabel()
        {
            if (Host.SelectedElement != this)
            {
                return;
            }

            UpdateTextProperty();

            UpdateFontWeight();
        }

        void UpdateTextProperty()
        {
            Text = Model?.Text;
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