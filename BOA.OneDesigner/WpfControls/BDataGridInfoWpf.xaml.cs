using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridInfoWpf.xaml
    /// </summary>
    public partial class BDataGridInfoWpf : IEventBusDisposable, IHostItem
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BCardWpf" /> class.
        /// </summary>
        public BDataGridInfoWpf()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                AttachToEventBus();
                Refresh();
            };
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the data.
        /// </summary>
        public BDataGrid BData => (BDataGrid) DataContext;

        public UIElementCollection ColumnsCollection => _columnsContainer.Children;

        public Host Host { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is in toolbox.
        /// </summary>
        public bool IsInToolbox { get; set; }
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);
        }

        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (IsVisible == false)
            {
                //UnSubscribeFromEventBus(); // TODO:?? 
                return;
            }

            _columnsContainer.Children.RemoveAll();

            if (BData == null)
            {
                return;
            }

            foreach (var columnInfo in BData.Columns)
            {
                var uiElement = new BDataGridColumnWpf(columnInfo, Host);

                Host.DragHelper.MakeDraggable(uiElement);

                _columnsContainer.Children.Add(uiElement);
            }
        }

        public void UnSubscribeFromEventBus()
        {
            _columnsContainer.Children.UnSubscribeFromEventBus();

            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, Refresh);
        }
        #endregion
    }
}