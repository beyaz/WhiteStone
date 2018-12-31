using System.Windows.Media;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridInfoWpf.xaml
    /// </summary>
    public partial class BDataGridInfoWpf : IEventBusDisposable
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BCardWpf" /> class.
        /// </summary>
        public BDataGridInfoWpf()
        {
            InitializeComponent();

            EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);

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

        #region Public Methods
        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            _columnsContainer.Children.RemoveAll();

            if (BData == null)
            {
                return;
            }

            foreach (var columnInfo in BData.Columns)
            {
                var uiElement = new BDataGridColumnWpf
                {
                    DataContext = columnInfo
                };

                _columnsContainer.Children.Add(uiElement);
            }
        }

        public void UnSubscribeFromEventBus()
        {
            EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, Refresh);
        }
        #endregion
    }
}