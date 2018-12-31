using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BInputWpf : Grid, IEventBusDisposable
    {
        #region Fields
        public TextBox   _bindingPath;
        public TextBlock _label;
        #endregion

        #region Constructors
        public BInputWpf()
        {
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);

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
        public BInput Data => (BInput) DataContext;
        #endregion

        #region Public Methods
        public void UnSubscribeFromEventBus()
        {
            EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
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