using System;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BInputWpf : Grid, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        #region Fields
        public TextBox   _bindingPath;
        public TextBlock _label;
        #endregion

        #region Constructors
        public BInputWpf()
        {
            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;

            this.LoadJson(@"
{
    Margin:10,
	rows:
	[
		{view:'TextBlock', Name:'_label',        Text:'{Binding " + nameof(BInput.Label) + @",       Mode = OneWay}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Name:'_bindingPath',  Text:'{Binding " + nameof(BInput.ValueBindingPath) + @", Mode = OneWay}' , IsReadOnly:true}        
	]
	
}");

            Loaded += (s, e) => { EvaluateRowCount(); };
        }
        #endregion

        #region Public Properties
        public Host   Host        { get; set; }
        public bool   IsInToolbox { get; set; }
        public BInput Model       => (BInput) DataContext;

        public SizeInfo SizeInfo => Model?.SizeInfo;
        #endregion

        #region Methods
        void BInput_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        void BInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        bool SelectedElementIsNotThisElement => Host.SelectedElement != this;


        void OnRowCountChanged()
        {
            if (SelectedElementIsNotThisElement)
            {
                return;
            }

            EvaluateRowCount();

        }

        void EvaluateRowCount()
        {
            if (Model.RowCount > 0)
            {
                _bindingPath.MinHeight = Model.RowCount.Value * 10;
            }
            else
            {
                _bindingPath.MinHeight = 10;
            }
        }

        void UpdateBindingPath() // TODO check
        {
            _bindingPath.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        void UpdateLabel()
        {
            if (SelectedElementIsNotThisElement)
            {
                return;
            }

            _label.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();
        }
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.LabelChanged, UpdateLabel);
            Host.EventBus.Subscribe(EventBus.RowCountChanged, OnRowCountChanged);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.LabelChanged, UpdateLabel);
            Host.EventBus.UnSubscribe(EventBus.RowCountChanged, OnRowCountChanged);
        }
        #endregion
    }
}