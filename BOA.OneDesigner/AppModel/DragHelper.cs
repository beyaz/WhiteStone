using System;
using System.Windows;
using System.Windows.Input;

// using BOA.OneDesigner.WpfControls;

namespace BOA.OneDesigner.AppModel
{
    public class DragHelper
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DragHelper" /> class.
        /// </summary>
        public DragHelper(Host host)
        {
            Host = host;
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        EventBus EventBus => Host.EventBus;
        #endregion

        #region Public Methods
      

        public void MakeDraggable(UIElement element)
        {
            element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;

            element.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;

            element.PreviewMouseMove += OnMouseMove;
        }
        #endregion

        #region Methods
        static bool IsInDragDistance(Point startPoint, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(null);
            var diff          = startPoint - mousePosition;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                return true;
            }

            return false;
        }

       

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            if (Host?.SelectedElement == null)
            {
                return;
            }

            if (!IsInDragDistance(Host.DraggingElementStartPoint, e))
            {
                return;
            }

            EventBus.Publish(EventBus.OnDragStarted);

            var dragData = new DataObject(string.Empty);

            DragDrop.DoDragDrop(Host.SelectedElement, dragData, DragDropEffects.Copy);
        }

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Host.DraggingElementStartPoint = e.GetPosition(null);
            Host.SelectedElement           = (UIElement) sender;
            Host.LastSelectedUIElement     = (UIElement) sender;

            EventBus.Publish(EventBus.BeforeDragElementSelected);

            EventBus.Publish(EventBus.OnDragElementSelected);
        }

        void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Host.SelectedElement == null)
            {
                return;
            }
            if (!IsInDragDistance(Host.DraggingElementStartPoint, e))
            {
                return;
            }
            
            EventBus.Publish(EventBus.OnAfterDropOperation);

            Host.SelectedElement = null;
        }
        #endregion
    }
}