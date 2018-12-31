using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     The b card WPF
    /// </summary>
    public class BCardWpf : Grid,IEventBusDisposable
    {

        public void UnSubscribeFromEventBus()
        {
            EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
            EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, RefreshTitle);
        }


        #region Fields
        /// <summary>
        ///     The children container
        /// </summary>
        public StackPanel ChildrenContainer;

        #pragma warning disable 649
        /// <summary>
        ///     The group box
        /// </summary>
        GroupBox _groupBox;
        #pragma warning restore 649
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BCardWpf" /> class.
        /// </summary>
        public BCardWpf()
        {
            EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, RefreshTitle);

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
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the data.
        /// </summary>
        public BCard Data => (BCard) DataContext;

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
        ///     Called when [drop].
        /// </summary>
        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var bInput = UIContext.DraggingElement as BInputWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                Data.InsertItem(insertIndex, bInput.Data);


                return;
            }

            throw new ArgumentException();
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
                    var uiElement = new BInputWpf
                    {
                        DataContext = bField
                    };

                    DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    continue;
                }

                throw new ArgumentException(bField.GetType().FullName);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Invoked whenever the effective value of any dependency property on this
        ///     <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is
        ///     reported in the arguments parameter. Overrides
        ///     <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />
        ///     .
        /// </summary>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        ///     Determines whether this instance can drop the specified drag element.
        /// </summary>
        static bool CanDrop(UIElement dragElement)
        {
            if (dragElement is BInputWpf)
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

            if (!CanDrop(UIContext.DraggingElement))
            {
                return;
            }

            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

            var items = ChildrenContainer.Children.ToArray();

            ChildrenContainer.Children.Clear();

            for (var i = 0; i < items.Length; i++)
            {
                var control = items[i];

                var dropLocation = new DropLocation {OnDropAction = OnDrop, TargetLocationIndex = i};

                ChildrenContainer.Children.Add(dropLocation);

                ChildrenContainer.Children.Add(control);
            }

            ChildrenContainer.Children.Add(new DropLocation {OnDropAction = OnDrop, TargetLocationIndex = items.Length});
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

            var items = ChildrenContainer.Children.ToArray();

            ChildrenContainer.Children.Clear();

            foreach (var control in items)
            {
                if (control is DropLocation)
                {
                    continue;
                }

                ChildrenContainer.Children.Add(control);
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