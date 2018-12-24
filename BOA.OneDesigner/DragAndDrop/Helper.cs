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

            var info = Info.Current;
            if (info == null)
            {
                return;
            }

            if (Equals(sender, info.Sender))
            {
                return;
            }

            // Get the current mouse position
            var mousePos = e.GetPosition(null);
            var diff     = info.StartPoint - mousePos;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var surface = (sender as DependencyObject)?.FindParent<JsxElementDesignerSurface>();
                if (surface != null)
                {
                    surface.EnterDropLocationMode();

                    var dragData = new DataObject("myFormat", "A");
                    DragDrop.DoDragDrop(info.Sender, dragData, DragDropEffects.Copy);
                }
            }
        }

        static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Info.Current = new Info
            {
                StartPoint = e.GetPosition(null),
                Sender     = (UIElement) sender
            };
        }
        #endregion
    }
}