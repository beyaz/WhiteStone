using System;
using System.Windows;
using System.Windows.Input;

namespace BOA.OneDesigner.WpfControls
{
    static class DragHelper
    {
        #region Public Methods
        public static void MakeDraggable(UIElement element)
        {
            element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;

            element.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;

            element.PreviewMouseMove += OnMouseMove;
        }
        #endregion

        #region Methods
        static void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var surfaceItem = sender as IJsxElementDesignerSurfaceItem;
            if (surfaceItem == null || surfaceItem.Surface == null)
            {
                return;
            }

            // Get the current mouse position
            var mousePos = e.GetPosition(null);
            var diff     = UIContext.DraggingElementStartPoint - mousePos;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                EventBus.OnDragStarted();

                var dragData = new DataObject(string.Empty);
                DragDrop.DoDragDrop(UIContext.DraggingElement, dragData, DragDropEffects.Copy);
            }
        }

        static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIContext.DraggingElementStartPoint = e.GetPosition(null);
            UIContext.DraggingElement           = (UIElement) sender;
            EventBus.OnDragElementSelected();
        }

        static void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIContext.DraggingElement = null;
        }
        #endregion
    }
}