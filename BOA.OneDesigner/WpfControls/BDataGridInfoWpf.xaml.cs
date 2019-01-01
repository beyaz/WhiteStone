using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridInfoWpf.xaml
    /// </summary>
    public partial class BDataGridInfoWpf : IEventBusDisposable,IHostItem
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BCardWpf" /> class.
        /// </summary>
        public BDataGridInfoWpf()
        {
            InitializeComponent();

            EventBus2.Subscribe(EventBus2.OnAfterDropOperation, Refresh);
            EventBus2.Subscribe(EventBus2.OnComponentPropertyChanged, Refresh);

            Background = Brushes.Bisque;

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is in toolbox.
        /// </summary>
        public bool IsInToolbox { get; set; }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the data.
        /// </summary>
        public BDataGrid BData => (BDataGrid) DataContext;
        #endregion


        public UIElementCollection ColumnsCollection => _columnsContainer.Children;

        #region Public Methods
        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (IsVisible == false)
            {
                //UnSubscribeFromEventBus();
                //return;
            }
            _columnsContainer.Children.RemoveAll();

            if (BData == null)
            {
                return;
            }

            foreach (var columnInfo in BData.Columns)
            {
                var uiElement = new BDataGridColumnWpf(columnInfo);

                _columnsContainer.Children.Add(uiElement);
            }
        }

        public void UnSubscribeFromEventBus()
        {
            _columnsContainer.Children.UnSubscribeFromEventBus();

            EventBus2.UnSubscribe(EventBus2.OnAfterDropOperation, Refresh);
            EventBus2.UnSubscribe(EventBus2.OnComponentPropertyChanged, Refresh);
        }
        #endregion

        public Host Host { get; set; }
    }
}