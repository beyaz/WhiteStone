using System;
using System.Windows;
using System.Windows.Input;
using BOA.OneDesigner.WpfControls;

namespace BOA.OneDesigner.DragAndDrop
{




    public interface IDropLocation
    {
        void OnDragEnter();

        void OnDragLeave();

        Action<IDropLocation> OnDropAction { get; }

         int TargetLocationIndex { get; set; }
    }

    public interface IDropLocationContainer
    {
        void EnterDropLocationMode();
        void ExitDropLocationMode();
    }

    static class Helper
    {
        #region Public Methods
        public static void MakeDraggable(UIElement element)
        {
            element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;

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
                (sender as IDropLocation)?.OnDragEnter();

                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        static void OnDragLeave(object sender, DragEventArgs e)
        {
            (sender as IDropLocation)?.OnDragLeave();
        }

        static void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                var dropLocation = (sender as IDropLocation);

                dropLocation?.OnDropAction(dropLocation);
            }
        }

        static void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            

            var surfaceItem = sender as IJsxElementDesignerSurfaceItem;
            if (surfaceItem == null)
            {
                return;
            }

            if (Equals(sender, surfaceItem.Surface.DraggingElement))
            {
                return;
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
            var surface = (sender as UIElement)?.FindParent<JsxElementDesignerSurface>();

            if (surface == null)
            {
                throw new ArgumentException();
            }

            surface.DraggingElementStartPoint = e.GetPosition(null);
            surface.DraggingElement = (UIElement) sender;
        }
        #endregion
    }
}