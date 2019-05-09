using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.PropertyEditors;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class PropertyEditorContainer : GroupBox, IHostItem
    {
        #region Constructors
        public PropertyEditorContainer()
        {
            Header = "Properties";

            Loaded   += (s, e) => { AttachToEventBus(); };
            Unloaded += (s, e) => { DeAttachToEventBus(); };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnDragElementSelected, Refresh);
            Host.EventBus.Subscribe(EventBus.ComponentDeleted, Refresh);
            Host.EventBus.Subscribe(EventBus.OnDragElementSelected, UpdateHeader);
        }

        public void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnDragElementSelected, Refresh);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Refresh);
            Host.EventBus.UnSubscribe(EventBus.OnDragElementSelected, UpdateHeader);
        }

        public void Refresh()
        {
            Content = null;

            var actionButton = Host.SelectedElement as ActionButton;
            if (actionButton != null)
            {
                DataContext = actionButton.Model;
                var resourceActionEditor = new ResourceActionEditor
                {
                    Host = Host,
                    DataContext = actionButton.Model
                };
                resourceActionEditor.LoadUI();
                

                SetContent(resourceActionEditor);
                return;
            }

            if (Host.SelectedElement == null)
            {
                DataContext = null;
                return;
            }

            DataContext = ((FrameworkElement) Host.SelectedElement).DataContext;

            if (DataContext == null)
            {
                return;
            }

            

            var dataGridColumnInfo = DataContext as BDataGridColumnInfo;
            if (dataGridColumnInfo != null)
            {
                SetContent(Host.Create<BDataGridColumnInfoEditor>(DataContext));
                return;
            }

            var bCard = DataContext as BCard;
            if (bCard != null)
            {
                SetContent(Host.Create<BCardEditor>(DataContext));
                return;
            }

            var dataGridInfo = DataContext as BDataGrid;
            if (dataGridInfo != null)
            {
                SetContent(BDataGridEditor.Create(Host,dataGridInfo));
                return;
            }

            var bComboBox = DataContext as BComboBox;
            if (bComboBox != null)
            {
                SetContent(Host.Create<BComboBoxEditor>(bComboBox));
                return;
            }

            var tabControl = DataContext as BTabBar;
            if (tabControl != null)
            {
                SetContent(Host.Create<BTabBarEditor>(tabControl));
                return;
            }

            var bTabBarPage = DataContext as BTabBarPage;
            if (bTabBarPage != null)
            {
                SetContent(Host.Create<BTabBarPageEditor>(DataContext));
                return;
            }

            var component = Host.SelectedElement as ComponentWpf;
            if (component != null)
            {
                SetContent(ComponentEditor.Create(Host, component.Model.Info));

                return;
            }

            throw new ArgumentException();
        }
        #endregion

        #region Methods
        void SetContent(object propertyEditor)
        {
            Content = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content                     = propertyEditor
            };
        }

        void UpdateHeader()
        {
            if (DataContext == null)
            {
                Header = "Properties";
                return;
            }

            var name = DataContext.GetType().Name;

            var component = Host.SelectedElement as ComponentWpf;
            if (component != null)
            {
                name = component.Model.Info.Type.GetName();
            }

            Header = "Properties -> " + name;
        }
        #endregion
    }
}