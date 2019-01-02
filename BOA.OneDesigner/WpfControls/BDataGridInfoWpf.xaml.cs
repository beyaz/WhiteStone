using System.Linq;
using System.Windows;
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

            Unloaded += (s, e) => { DeAttachToEventBus(); };
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
        ///     Gets or sets a value indicating whether this instance is entered drop location mode.
        /// </summary>
        public bool IsEnteredDropLocationMode { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is in toolbox.
        /// </summary>
        public bool IsInToolbox { get; set; }
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);

            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);

            Host.EventBus.Subscribe(EventBus.DataGridColumnRemoved, OnColumnRemoved);
        }

        public void DeAttachToEventBus()
        {
            _columnsContainer.Children.UnSubscribeFromEventBus();

            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, Refresh);
        }

        /// <summary>
        ///     Called when [drop].
        /// </summary>
        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var dataGridColumnWpf = Host.DraggingElement as BDataGridColumnWpf;
            if (dataGridColumnWpf != null)
            {
                var dataGridColumnInfo = dataGridColumnWpf.DataContext as BDataGridColumnInfo;

                BData.Columns.Remove(dataGridColumnInfo);

                BData.Columns.Insert(insertIndex, dataGridColumnInfo);

                return;
            }

            throw Error.InvalidOperation();
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
                var uiElement = new BDataGridColumnWpf(columnInfo, Host, this);

                Host.DragHelper.MakeDraggable(uiElement);

                _columnsContainer.Children.Add(uiElement);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether this instance can drop the specified drag element.
        /// </summary>
        bool CanDrop(UIElement dragElement)
        {
            return _columnsContainer.Children.ToArray().Contains(dragElement);
        }

        /// <summary>
        ///     Enters the drop location mode.
        /// </summary>
        void EnterDropLocationMode()
        {
            if (IsInToolbox)
            {
                return;
            }

            if (!CanDrop(Host.DraggingElement))
            {
                return;
            }

            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

            var children = _columnsContainer.Children;

            var items = children.ToArray();

            children.Clear();

            for (var i = 0; i < items.Length; i++)
            {
                var control = items[i];

                var dropLocation = new DropLocation
                {
                    Host                = Host,
                    OnDropAction        = OnDrop,
                    TargetLocationIndex = i
                };

                children.Add(dropLocation);

                children.Add(control);
            }

            children.Add(new DropLocation
            {
                Host                = Host,
                OnDropAction        = OnDrop,
                TargetLocationIndex = items.Length
            });
        }

        void ExitDropLocationMode()
        {
            if (!IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = false;

            var children = _columnsContainer.Children;

            var items = children.ToArray();

            children.Clear();

            foreach (var control in items)
            {
                if (control is DropLocation)
                {
                    continue;
                }

                children.Add(control);
            }
        }

        void OnColumnRemoved()
        {
            var column = BData.Columns.FirstOrDefault(x => x.ShouldRemove);
            if (column != null)
            {
                var index = BData.Columns.IndexOf(column);
                if (index >= 0)
                {
                    BData.Columns.RemoveAt(index);
                    Refresh();
                }
            }
        }
        #endregion
    }
}