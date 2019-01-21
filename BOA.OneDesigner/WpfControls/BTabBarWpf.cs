using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class BTabBarWpf : Border, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        #region Fields
        internal readonly WrapPanel HeadersContainersWrapPanel = new WrapPanel
        {
            Background = Brushes.LightSkyBlue
        };

        internal readonly StackPanel TabPageBodyList = new StackPanel();
        #endregion

        #region Constructors
        public BTabBarWpf()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(HeadersContainersWrapPanel);
            stackPanel.Children.Add(TabPageBodyList);

            Child = stackPanel;
        }
        #endregion

        #region Public Properties
        public Host    Host        { get; set; }
        public bool    IsInToolbox { get; set; }
        public BTabBar Model       => (BTabBar) DataContext;

        public SizeInfo SizeInfo => Model.SizeInfo;

        public int TabCount => HeadersContainersWrapPanel.Children.Count;
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
                throw Error.InvalidOperation();
            }

            foreach (var bTabBarPage in Model.Items)
            {
                var uiElement = new BTabBarPageWpf(bTabBarPage, Host, this);

                Host.DragHelper.MakeDraggable(uiElement);

                var tabPageBody = Host.CreateDivAsCardContainerWpf(bTabBarPage.DivAsCardContainer);

                tabPageBody.MinHeight = 100;
                tabPageBody.MinWidth  = 150;

                if (Model.Items.Count == 1)
                {
                    bTabBarPage.IsActiveInDesigner = true;
                }

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
                    if (bTabBarPage.IsActiveInDesigner)
                    {
                        return;
                    }

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
            var isUpdated = UpdateModel(dropLocation.TargetLocationIndex);
            if (isUpdated)
            {
                Refresh();
            }
        }

        void OnTabPageRemoved()
        {
            var bTabBarPage = (Host.SelectedElement as BTabBarPageWpf)?.Model;

            if (Model.Items.Contains(bTabBarPage) && Model.Items.Count == 1)
            {
                App.ShowErrorNotification("En az bir tab olmalıdır.");
                return;
            }

            var isRemoved = Model.Items.Remove(bTabBarPage);

            if (isRemoved)
            {
                Refresh();
            }
        }

        void OnTabPageAdded()
        {

            if (Host.SelectedElement != this)
            {
                return;
            }

            Model.Items.Add(new BTabBarPage
            {
                TitleInfo = LabelInfoHelper.CreateNewLabelInfo("Page " + Model.Items.Count)
            });

            Refresh();
        }

        


        bool UpdateModel(int insertIndex)
        {
            var bTabBarPageWpf = Host.SelectedElement as BTabBarPageWpf;
            if (bTabBarPageWpf != null)
            {
                if (Model.Items.Contains(bTabBarPageWpf.Model))
                {
                    InsertHelper.Move(Model.Items, bTabBarPageWpf.Model, insertIndex);
                    return true;
                }

                return false;
            }

            throw Error.InvalidOperation();
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

            Host.EventBus.Subscribe(EventBus.TabBarPageRemoved, OnTabPageRemoved);
            Host.EventBus.Subscribe(EventBus.TabBarPageAdded, OnTabPageAdded);
            
        }

        public void DeAttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnDeAttachToEventBus?.Invoke();
            Host.EventBus.UnSubscribe(EventBus.TabBarPageRemoved, OnTabPageRemoved);
            Host.EventBus.UnSubscribe(EventBus.TabBarPageAdded, OnTabPageAdded);
        }
        #endregion
    }
}