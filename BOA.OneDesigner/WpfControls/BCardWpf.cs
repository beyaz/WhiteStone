using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     The b card WPF
    /// </summary>
    public class BCardWpf : Grid, IHostItem, IEventBusListener
    {

        void Should_delete_and_refresh_if_my_child_selected()
        {
            var willBeDeleteElement = ChildrenContainer.Children.ToArray().FirstOrDefault(x=>x==Host.SelectedElement);
            if (willBeDeleteElement == null)
            {
                return;
            }


            var bInput = Host.SelectedElement as BInputWpf;
            var dataGridInfoWpf = Host.SelectedElement as BDataGridInfoWpf;
            var tabControlWpf = Host.SelectedElement as BTabBarWpf;
            var bComboBoxInWpf = Host.SelectedElement as BComboBoxInWpf;


            if (bInput != null)
            {
                bInput.Model.RemoveFromParent();
            }
            else  if (dataGridInfoWpf != null)
            {
                dataGridInfoWpf.Model.RemoveFromParent();
            }
            else if (tabControlWpf != null)
            {
                tabControlWpf.Model.RemoveFromParent();
            }
            else if (bComboBoxInWpf != null)
            {
                bComboBoxInWpf.Model.RemoveFromParent();
            }
            else
            {
                throw Error.InvalidOperation();
            }

            Refresh();
        }


        #region Fields
        /// <summary>
        ///     The group box
        /// </summary>
        public GroupBox _groupBox;

        /// <summary>
        ///     The children container
        /// </summary>
        public Grid ChildrenContainer;
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
            Content: { ui:'Grid' , Name:'" + nameof(ChildrenContainer) + @"' }
        }
	]
	
}");

        }
        #endregion

        #region Public Properties
        public int BChildrenCount => ChildrenContainer.Children.Count;

        public UIElement BChildAt(int index)
        {
            return ChildrenContainer.Children[index];
        }

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

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public BCard Model => (BCard) DataContext;
        #endregion

        #region Properties
        internal string HeaderAsString => _groupBox.Header as string;
        #endregion

        #region Public Methods
       

        /// <summary>
        ///     Called when [drop].
        /// </summary>
        public void OnDrop(DropLocation dropLocation)
        {
            UpdateModel(dropLocation.TargetLocationIndex);
            Refresh();
        }

        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            Host.DeAttachToEventBus(ChildrenContainer.Children);

            ChildrenContainer.Children.Clear();

            if (Model == null)
            {
                return;
            }

            foreach (var bField in Model.Items)
            {
                var bInput = bField as BInput;
                if (bInput!= null)
                {
                    bInput.Container = Model;
                    var uiElement = Host.Create<BInputWpf>(bInput);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    Host.AttachToEventBus(uiElement, this);

                    continue;
                }

                var bLabel = bField as BLabel;
                if (bLabel!= null)
                {
                    bLabel.Container = Model;
                    var uiElement = Host.Create<BLabelWpf>(bLabel);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    Host.AttachToEventBus(uiElement, this);

                    continue;
                }

                var componentInfo = bField as ComponentInfo;
                if (componentInfo!= null)
                {
                    componentInfo.Container = Model;
                    var uiElement = ComponentWpf.Create(Host, componentInfo);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    Host.AttachToEventBus(uiElement, this);

                    continue;
                }


                var bComboBox = bField as BComboBox;
                if (bComboBox!= null)
                {
                    bComboBox.Container = Model;
                    var uiElement = Host.Create<BComboBoxInWpf>(bComboBox);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    Host.AttachToEventBus(uiElement, this);

                    continue;
                }

                var bDataGrid = bField as BDataGrid;
                if (bDataGrid != null)
                {
                    bDataGrid.Container = Model;
                    var uiElement = Host.CreateBDataGridInfoWpf(bDataGrid);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                    Host.AttachToEventBus(uiElement, this);
                    continue;
                }

                var bTabBar = bField as BTabBar;
                if (bTabBar != null)
                {
                    bTabBar.Container = Model;
                    var uiElement = Host.CreateBTabBarWpf(bTabBar);

                    Host.DragHelper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);

                    Host.AttachToEventBus(uiElement, this);
                    continue;
                }

                throw Error.InvalidOperation(bField.GetType().FullName);
            }

            CardLayout.Apply(ChildrenContainer);
        }
        #endregion


        void Should_ExitDropLocationMode_when_any_component_selected()
        {
            ExitDropLocationMode();
        }

        void Should_refresh_when_any_my_child_component_moved_or_deleted()
        {
            var isMyChild = Children.Contains(Host.SelectedElement);
            if (isMyChild)
            {
                Refresh();
            }
        }




        #region Methods
        /// <summary>
        ///     Enters the drop location mode.
        /// </summary>
        internal void EnterDropLocationMode()
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

            // Host.DeAttachToEventBus(ChildrenContainer.Children);

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
                    TargetLocationIndex = i,
                    Margin              = new Thickness(20)
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

            // Host.AttachToEventBus(children);

            
            Action action = () => { CardLayout.ApplyWithDropLocationMode(ChildrenContainer); };
            Dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }

        /// <summary>
        ///     Exits the drop location mode.
        /// </summary>
        internal void ExitDropLocationMode()
        {
            if (!IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = false;

            //Host.DeAttachToEventBus(ChildrenContainer.Children);

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

            // Host.AttachToEventBus(ChildrenContainer.Children);

            CardLayout.Apply(ChildrenContainer);
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

            if (dragElement is BLabelWpf)
            {
                return true;
            }

            if (dragElement is BComboBoxInWpf)
            {
                return true;
            }

            if (dragElement is BDataGridInfoWpf)
            {
                return true;
            }

            if (dragElement is BTabBarWpf)
            {
                return true;
            }

            if (dragElement is ComponentWpf)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Refreshes the title.
        /// </summary>
        void RefreshTitle()
        {
            _groupBox.GetBindingExpression(HeaderedContentControl.HeaderProperty)?.UpdateTarget();
        }

        void UpdateModel(int insertIndex)
        {
            var bInput = Host.SelectedElement as BInputWpf;
            if (bInput != null)
            {
                if (bInput.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BInput());
                    return;
                }

                bInput.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, bInput.Model);

                return;
            }

            var bLabelInWpf = Host.SelectedElement as BLabelWpf;
            if (bLabelInWpf != null)
            {

                if (bLabelInWpf.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BLabel());
                    return;
                }

                bLabelInWpf.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, bLabelInWpf.Model);

                return;
            }


            var component = Host.SelectedElement as ComponentWpf;
            if (component != null)
            {
                if (component.Model.IsInToolbox)
                {
                    if (component.Model.Info.Type.IsDivider)
                    {
                        Model.InsertItem(insertIndex, new ComponentInfo{Type = new ComponentType{IsDivider = true}});    
                        return;
                    }
                }
                
                component.Model.Info.RemoveFromParent();

                Model.InsertItem(insertIndex, component.Model.Info);

                return;
            }


            var bComboBoxInWpf = Host.SelectedElement as BComboBoxInWpf;
            if (bComboBoxInWpf != null)
            {
                if (bComboBoxInWpf.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BComboBox());
                    return;
                }

                bComboBoxInWpf.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, bComboBoxInWpf.Model);

                return;
            }


            var dataGridInfoWpf = Host.SelectedElement as BDataGridInfoWpf;
            if (dataGridInfoWpf != null)
            {
                if (dataGridInfoWpf.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BDataGrid());
                    return;
                }

                dataGridInfoWpf.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, dataGridInfoWpf.Model);

                return;
            }

            var tabControlWpf = Host.SelectedElement as BTabBarWpf;
            if (tabControlWpf != null)
            {
                if (tabControlWpf.IsInToolbox)
                {
                    Model.InsertItem(insertIndex, new BTabBar
                    {
                        Items = new List<BTabBarPage>
                        {
                            new BTabBarPage
                            {
                                TitleInfo = LabelInfoHelper.CreateNewLabelInfo("Page 0")
                            }
                        }
                    });
                    return;
                }

                tabControlWpf.Model.RemoveFromParent();

                Model.InsertItem(insertIndex, tabControlWpf.Model);

                return;
            }

            throw Error.InvalidOperation();
        }
        #endregion

        bool SelectedElementIsMyChild => ChildrenContainer.Children.Contains(Host.SelectedElement);

        void OnSizeChanged()
        {
            if (!SelectedElementIsMyChild)
            {
                return;
            }

            CardLayout.Apply(ChildrenContainer);
        }

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Should_refresh_when_any_my_child_component_moved_or_deleted);
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.Subscribe(EventBus.OnDragElementSelected, Should_ExitDropLocationMode_when_any_component_selected);
            Host.EventBus.Subscribe(EventBus.ComponentDeleted, Should_delete_and_refresh_if_my_child_selected);
            Host.EventBus.Subscribe(EventBus.LabelChanged, RefreshTitle);
            Host.EventBus.Subscribe(EventBus.SizeChanged, OnSizeChanged);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.OnDragStarted, EnterDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Should_refresh_when_any_my_child_component_moved_or_deleted);
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, ExitDropLocationMode);
            Host.EventBus.UnSubscribe(EventBus.OnDragElementSelected, Should_ExitDropLocationMode_when_any_component_selected);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Should_delete_and_refresh_if_my_child_selected);
            Host.EventBus.UnSubscribe(EventBus.LabelChanged, RefreshTitle);
            Host.EventBus.UnSubscribe(EventBus.SizeChanged, OnSizeChanged);
        }
        #endregion
    }
}