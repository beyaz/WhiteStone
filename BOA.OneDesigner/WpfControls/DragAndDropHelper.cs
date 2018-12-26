﻿using System;
using System.Windows;
using System.Windows.Input;

namespace BOA.OneDesigner.WpfControls
{
    static class DragAndDropHelper
    {
        #region Public Methods
        public static void MakeDraggable(UIElement element)
        {
            element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            element.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;

            element.PreviewMouseMove += OnMouseMove;
        }

        public static void MakeDropLocation(UIElement element)
        {
            element.AllowDrop =  true;

            element.Drop      += OnDrop;

            element.DragEnter += OnDragEnter;

            element.DragLeave += OnDragLeave;
        }
        #endregion

        #region Methods
        static void OnDragEnter(object sender, DragEventArgs e)
        {
            if (sender == e.Source)
            {
                (sender as DropLocation)?.OnDragEnter();

                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        static void OnDragLeave(object sender, DragEventArgs e)
        {
            (sender as DropLocation)?.OnDragLeave();
        }

        static void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                var dropLocation = (sender as DropLocation);

                dropLocation?.OnDropAction(dropLocation);

                AfterDropOperation?.Invoke();
            }
        }

        public static event Action AfterDropOperation;

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

            if (Equals(sender, surfaceItem.Surface.DraggingElement))
            {
                // return;
            }

            // Get the current mouse position
            var mousePos = e.GetPosition(null);
            var diff     = surfaceItem.Surface.DraggingElementStartPoint - mousePos;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var surface = surfaceItem.Surface;

                surface.EnterDropLocationMode();

                var dragData = new DataObject("myFormat", "A");
                DragDrop.DoDragDrop(surfaceItem.Surface.DraggingElement, dragData, DragDropEffects.Copy);
            }
        }

       

        static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

           UIContext. DraggingElementStartPoint = e.GetPosition(null);
           UIContext.DraggingElement           = (UIElement) sender;
           UIContext.OnDragElementSelected();

        }

        static void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            UIContext.DraggingElement            = null;

        }
        #endregion

        

        
    }
}