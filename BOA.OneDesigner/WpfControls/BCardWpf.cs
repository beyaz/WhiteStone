using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     The b card WPF
    /// </summary>
    public class BCardWpf : Grid, IEventBusDisposable, IHostItem
    {

        public UIElement BChildrenAt(int index)
        {
            return ChildrenContainer.Children[index];
        }

        public int BChildrenCount => ChildrenContainer.Children.Count;

        #region Fields
        /// <summary>
        ///     The group box
        /// </summary>
        public GroupBox _groupBox;

        /// <summary>
        ///     The children container
        /// </summary>
        public StackPanel ChildrenContainer;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BCardWpf" /> class.
        /// </summary>
        public BCardWpf()
        {
            Background = Brushes.White;

            this.LoadJson(@"
{
	rows:
	[
		{
            view:   'GroupBox', Name:'_groupBox',
            Header: '{Binding " + nameof(BCard.Title) + @",Mode=OneWay}', 
            Content: { ui:'StackPanel' , Name:'" + nameof(ChildrenContainer) + @"' }
        }
	]
	
}");

            Loaded += (s, e) => { AttachToEventBus(); };
            Unloaded += (s, e) => { DeAttachToEventBus(); };

            Loaded += (s, e) => { Refresh(); };
        }

        
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the data.
        /// </summary>
        public BCard Data => (BCard) DataContext;

        /// <summary>
        ///     Gets or sets the host.
        /// </summary>
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
        /// <summary>
        ///     Attaches to event bus.
        /// </summary>
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.Subscribe(EventBus.ComponentDeleted, Refresh);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, RefreshTitle);
        }

        /// <summary>
        ///     Called when [drop].
        /// </summary>
        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var bInput = Host.DraggingElement as BInputWpf;
            if (bInput != null)
            {
                bInput.Model.RemoveFromParent();

                Data.InsertItem(insertIndex, bInput.Model);

                return;
            }

            var dataGridInfoWpf = Host.DraggingElement as BDataGridInfoWpf;
            if (dataGridInfoWpf != null)
            {
                dataGridInfoWpf.BData.RemoveFromParent();

                Data.InsertItem(insertIndex, dataGridInfoWpf.BData);

                return;
            }

            throw Error.InvalidOperation();
        }

        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            ChildrenContainer.Children.RemoveAll();

            if (Data == null)
            {
                return;
            }

            foreach (var bField in Data.Items)
            {
                if (bField is BInput)
                {
                    var uiElement = Host.Create<BInputWpf>(bField);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    continue;
                }

                if (bField is BDataGrid)
                {
                    var uiElement = Host.Create<BDataGridInfoWpf>(bField);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    continue;
                }

                throw Error.InvalidOperation(bField.GetType().FullName);
            }
        }

        /// <summary>
        ///     Uns the subscribe from event bus.
        /// </summary>
        public void DeAttachToEventBus()
        {
            ChildrenContainer.Children.UnSubscribeFromEventBus();

            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Refresh);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, RefreshTitle);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether this instance can drop the specified drag element.
        /// </summary>
        static bool CanDrop(UIElement dragElement)
        {
            if (dragElement is BInputWpf)
            {
                return true;
            }

            if (dragElement is BDataGridInfoWpf)
            {
                return true;
            }

            return false;
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

            var children = ChildrenContainer.Children;

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

        /// <summary>
        ///     Exits the drop location mode.
        /// </summary>
        void ExitDropLocationMode()
        {
            if (!IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = false;

            var children = ChildrenContainer.Children;

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

        /// <summary>
        ///     Refreshes the title.
        /// </summary>
        void RefreshTitle()
        {
            _groupBox.GetBindingExpression(HeaderedContentControl.HeaderProperty)?.UpdateTarget();
        }
        #endregion
    }
}