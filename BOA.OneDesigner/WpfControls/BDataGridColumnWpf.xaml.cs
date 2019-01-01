using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridColumnWpf.xaml
    /// </summary>
    public partial class BDataGridColumnWpf : IEventBusDisposable
    {
        #region Constructors
        public BDataGridColumnWpf(BDataGridColumnInfo dataContext)
        {
            DataContext = dataContext;

            InitializeComponent();

            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);

            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;
        }
        #endregion

        #region Properties
        BDataGridColumnInfo BData => (BDataGridColumnInfo) DataContext;
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
            _label.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }
        #endregion
    }
}