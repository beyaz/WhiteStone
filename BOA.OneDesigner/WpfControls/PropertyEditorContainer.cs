using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.PropertyEditors;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class PropertyEditorContainer : GroupBox
    {
        #region Constructors
        public PropertyEditorContainer()
        {
            Header = "Properties";

            EventBus2.Subscribe(EventBus2.OnDragElementSelected, Refresh);
        }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            Content = null;

            if (UIContext.SelectedElement == null)
            {
                DataContext = null;
                return;
            }

            DataContext = ((FrameworkElement) UIContext.SelectedElement).DataContext;

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
                Content = new BCardEditor {DataContext = DataContext};
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
    }
}