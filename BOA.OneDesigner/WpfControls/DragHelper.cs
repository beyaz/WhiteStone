﻿using System;
using System.Windows;
using System.Windows.Input;

namespace BOA.OneDesigner.WpfControls
{
    static class DragHelper
    {
        #region Constructors
        static DragHelper()
        {
            EventBus.AfterDropOperation += () => { UIContext.DraggingElement = null; };
        }
        #endregion

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

           

            var mousePosition = e.GetPosition(null);
            var diff          = UIContext.DraggingElementStartPoint - mousePosition;

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
            EventBus.OnAfterDropOperation();
        }
        #endregion
    }
}