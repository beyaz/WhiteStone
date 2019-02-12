﻿using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.MainForm;
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
                Content = new ResourceActionEditor
                {
                    DataContext = actionButton.Model
                };
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

            var bInput = DataContext as BInput;
            if (bInput != null)
            {
                Content = Host.Create<BInputEditor>(DataContext);
                return;
            }

            var bLabel = DataContext as BLabel;
            if (bLabel != null)
            {
                Content = Host.Create<BLabelEditor>(DataContext);
                return;
            }

            var dataGridColumnInfo = DataContext as BDataGridColumnInfo;
            if (dataGridColumnInfo != null)
            {
                Content = Host.Create<BDataGridColumnInfoEditor>(DataContext);
                return;
            }

            var bCard = DataContext as BCard;
            if (bCard != null)
            {
                Content = Host.Create<BCardEditor>(DataContext);
                return;
            }

            var dataGridInfo = DataContext as BDataGrid;
            if (dataGridInfo != null)
            {
                Content = Host.Create<BDataGridEditor>(dataGridInfo);
                return;
            }

            var bComboBox = DataContext as BComboBox;
            if (bComboBox != null)
            {
                Content = Host.Create<BComboBoxEditor>(bComboBox);
                return;
            }

            var tabControl = DataContext as BTabBar;
            if (tabControl != null)
            {
                Content = Host.Create<BTabBarEditor>(tabControl);
                return;
            }

            var bTabBarPage = DataContext as BTabBarPage;
            if (bTabBarPage != null)
            {
                Content = Host.Create<BTabBarPageEditor>(DataContext);
                return;
            }

            var component = Host.SelectedElement as Component2;
            if (component != null)
            {
                Content = ComponentEditor.Create(component.Model.Info);

                return;
            }

            throw new ArgumentException();
        }
        #endregion

        #region Methods
        void UpdateHeader()
        {
            if (DataContext == null)
            {
                Header = "Properties";
                return;
            }

            Header = "Properties -> " + DataContext.GetType().Name;
        }
        #endregion
    }
}