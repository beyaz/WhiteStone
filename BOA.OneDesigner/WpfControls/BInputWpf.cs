using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{

    public interface ISupportSizeInfo
    {
        SizeInfo SizeInfo { get; }
    }
    public class BInputWpf : Grid,  IHostItem,ISupportSizeInfo
    {
        #region Fields
        public TextBox   _bindingPath;
        public TextBlock _label;
        #endregion

        #region Constructors
        public BInputWpf()
        {
            Loaded += (s, e) => { AttachToEventBus(); };

            Unloaded += (s, e) => { DeAttachToEventBus(); };

            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;

            this.LoadJson(@"
{
    Margin:10,
	rows:
	[
		{view:'TextBlock', Name:'_label',        Text:'{Binding " + nameof(BInput.Label) + @",       Mode = OneWay}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Name:'_bindingPath',  Text:'{Binding " + nameof(BInput.BindingPath) + @", Mode = OneWay}' , IsReadOnly:true}        
	]
	
}");
        }
        #endregion

        #region Public Properties
        public BInput Model => (BInput) DataContext;

        public SizeInfo SizeInfo => Model?.SizeInfo;

        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }

        public void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }
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

        void UpdateBindingPath()
        {
            _bindingPath.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        void UpdateLabel()
        {
            _label.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();
        }
        #endregion
    }
}