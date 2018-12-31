using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     The b card WPF
    /// </summary>
    public class DataGridInfoWpf : WrapPanel, IEventBusDisposable
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BCardWpf" /> class.
        /// </summary>
        public DataGridInfoWpf()
        {
            EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);
            MinWidth  = 200;
            MinHeight = 70;

            Background = Brushes.Bisque;

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the data.
        /// </summary>
        public BDataGrid BData => (BDataGrid) DataContext;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is in toolbox.
        /// </summary>
        public bool IsInToolbox { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            Children.RemoveAll();

            if (BData == null)
            {
                return;
            }

            foreach (var columnInfo in BData.Columns)
            {
                var uiElement = new BDataGridColumnInfoWpf
                {
                    DataContext = columnInfo
                };

                Children.Add(uiElement);
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