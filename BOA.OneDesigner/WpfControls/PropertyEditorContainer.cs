using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.PropertyEditors;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class PropertyEditorContainer : GroupBox,IHostItem
    {
        
        #region Constructors
        public PropertyEditorContainer()
        {
            Header = "Properties";

            this.Loaded += (s, e) => { AttachToEventBus(); };

        }
        #endregion


        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnDragElementSelected, Refresh);
        }

        #region Public Methods
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
                Content = new BInputEditor {DataContext = DataContext};
                return;
            }

            var bCard = DataContext as BCard;
            if (bCard != null)
            {
                Content = new BCardEditor {DataContext = DataContext,Host = Host};
                return;
            }

            var dataGridInfo = DataContext as BDataGrid;
            if (dataGridInfo != null)
            {
                var editor = new BDataGridEditor
                {
                    Model = new BDataGridEditorModel
                    {
                        Info = dataGridInfo
                    }
                };

                editor.DataContext = editor.Model;

                Content = editor;
                return;
            }

            throw new ArgumentException();
        }
        #endregion

        public Host Host { get; set; }
    }
}