using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    class BComboBoxInWpf:StackPanel, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        public Host Host                      { get; set; }
        public bool IsInToolbox               { get; set; }
        public bool IsEnteredDropLocationMode { get; set; }

        public BComboBox Model => (BComboBox) DataContext;

        public SizeInfo SizeInfo { get; } = new SizeInfo {IsMedium = true};

        #region Public Methods
        /// <summary>
        ///     Called when [drop].
        /// </summary>
        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var dataGridColumnWpf = Host.SelectedElement as BDataGridColumnWpf;
            if (dataGridColumnWpf != null)
            {
                if (Model.Columns.Contains(dataGridColumnWpf.Model))
                {
                    InsertHelper.Move(Model.Columns, dataGridColumnWpf.Model, insertIndex);
                }

                return;
            }

            throw Error.InvalidOperation();
        }

        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (IsInToolbox)
            {
                return;
            }

            Host.DeAttachToEventBus(_columnsContainer.Children);

            _columnsContainer.Children.Clear();

            if (Model == null)
            {
                return;
            }

            foreach (var columnInfo in Model.Columns)
            {
                var uiElement = new BDataGridColumnWpf(columnInfo, Host, this);

                Host.DragHelper.MakeDraggable(uiElement);

                _columnsContainer.Children.Add(uiElement);

                Host.AttachToEventBus(uiElement, this);
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

            if (!CanDrop(Host.SelectedElement))
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
            if (ColumnsCollection.Contains(Host.SelectedElement) == false)
            {
                return;
            }

            var bDataGridColumnInfo = ((BDataGridColumnWpf) Host.SelectedElement).Model;

            Model.Columns.Remove(bDataGridColumnInfo);
            Refresh();
        }
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);
            Host.EventBus.Subscribe(EventBus.DataGridColumnRemoved, OnColumnRemoved);
        }

        public void DeAttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnDeAttachToEventBus?.Invoke();
            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, Refresh);
            Host.EventBus.UnSubscribe(EventBus.DataGridColumnRemoved, OnColumnRemoved);
        }
        #endregion
    }
}