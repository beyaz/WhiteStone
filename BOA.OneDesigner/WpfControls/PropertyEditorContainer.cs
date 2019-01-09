﻿using System;
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

            Loaded += (s, e) => { AttachToEventBus(); };
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
        }
        public void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnDragElementSelected, Refresh);
            Host.EventBus.UnSubscribe(EventBus.ComponentDeleted, Refresh);
        }

        public void Refresh()
        {
            Content = null;

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


            var tabControl= DataContext as BTabBar;
            if (tabControl != null)
            {
                Content = Host.Create<BDataGridEditor>(tabControl);
                return;
            }


            throw new ArgumentException();
        }
        #endregion
    }
}