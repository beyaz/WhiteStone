using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridColumnWpf.xaml
    /// </summary>
    public partial class BDataGridColumnWpf : IEventBusDisposable, IHostItem
    {
        #region Constructors
        public BDataGridColumnWpf(BDataGridColumnInfo dataContext, Host host)
        {
            DataContext = dataContext;
            Host        = host;

            InitializeComponent();

            Loaded += (s, e) => { AttachToEventBus(); };

            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        BDataGridColumnInfo BData => (BDataGridColumnInfo) DataContext;
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);

            
        }

        public void UnSubscribeFromEventBus()
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
            _label.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }
        #endregion
    }
}