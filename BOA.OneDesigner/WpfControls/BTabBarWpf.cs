using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class BTabBarWpf : Border, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        #region Fields
        internal readonly WrapPanel  HeadersContainersWrapPanel = new WrapPanel();
        internal readonly StackPanel TabPageBodyList            = new StackPanel();
        #endregion

        #region Constructors
        public BTabBarWpf()
        {
            BorderBrush     = Brushes.Black;
            BorderThickness = new Thickness(3);

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(HeadersContainersWrapPanel);
            stackPanel.Children.Add(TabPageBodyList);

            Child = stackPanel;

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        public Host    Host        { get; set; }
        public bool    IsInToolbox { get; set; }
        public BTabBar Model       => (BTabBar) DataContext;

        public SizeInfo SizeInfo { get; } = new SizeInfo {IsMedium = true};

        public int      TabCount => HeadersContainersWrapPanel.Children.Count;
        #endregion

        #region Methods
        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        internal void Refresh()
        {
            Host.DeAttachToEventBus(TabPageBodyList.Children);

            TabPageBodyList.Children.Clear();

            if (IsInToolbox)
            {
                TabPageBodyList.Children.Add(new GroupBox
                {
                    Header            = "Tab Bar",
                    VerticalAlignment = VerticalAlignment.Stretch
                });
                return;
            }

            Host.DeAttachToEventBus(HeadersContainersWrapPanel.Children);

            HeadersContainersWrapPanel.Children.Clear();

            if (Model == null)
            {
                return;
            }

            foreach (var bTabBarPage in Model.Items)
            {
                var uiElement = new BTabBarPageWpf(bTabBarPage, Host, this);

                Host.DragHelper.MakeDraggable(uiElement);

                var tabPageBody = Host.Create<DivAsCardContainerWpf>(bTabBarPage.DivAsCardContainer);

                tabPageBody.MinHeight = 100;
                tabPageBody.MinWidth  = 150;

                if (bTabBarPage.IsActiveInDesigner)
                {
                    tabPageBody.Visibility = Visibility.Visible;
                }
                else
                {
                    tabPageBody.Visibility = Visibility.Collapsed;
                }

                uiElement.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    Model.Items.ForEach(page => page.IsActiveInDesigner = false);
                    bTabBarPage.IsActiveInDesigner = true;
                    Refresh();
                };

                TabPageBodyList.Children.Add(tabPageBody);
                HeadersContainersWrapPanel.Children.Add(uiElement);

                Host.AttachToEventBus(tabPageBody, this);
                Host.AttachToEventBus(uiElement, this);
            }
        }

        /// <summary>
        ///     Called when [drop].
        /// </summary>
        void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var bTabBarPageWpf = Host.SelectedElement as BTabBarPageWpf;
            if (bTabBarPageWpf != null)
            {
                if (Model.Items.Contains(bTabBarPageWpf.Model))
                {
                    InsertHelper.Move(Model.Items, bTabBarPageWpf.Model, insertIndex);
                    Refresh();
                }

                return;
            }

            throw Error.InvalidOperation();
        }

        void OnTabPageRemoved()
        {
            var column = Model.Items.FirstOrDefault(x => x.ShouldRemove);
            if (column != null)
            {
                var index = Model.Items.IndexOf(column);
                if (index >= 0)
                {
                    Model.Items.RemoveAt(index);
                    Refresh();
                }
            }
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

            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, Refresh);
            Host.EventBus.Subscribe(EventBus.TabBarPageRemoved, OnTabPageRemoved);
        }

        public void DeAttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnDeAttachToEventBus?.Invoke();
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, Refresh);
            Host.EventBus.UnSubscribe(EventBus.TabBarPageRemoved, OnTabPageRemoved);
        }
        #endregion
    }
}