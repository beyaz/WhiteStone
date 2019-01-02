using System;
using System.Windows;
using System.Windows.Input;

namespace BOA.OneDesigner.AppModel
{
    public class DragHelper
    {
        #region Constructors
        public DragHelper(Host host)
        {
            Host = host;

            EventBus.Subscribe(EventBus.OnAfterDropOperation, () => { Host.DraggingElement = null; });
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
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var mousePosition = e.GetPosition(null);
            var diff          = Host.DraggingElementStartPoint - mousePosition;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                EventBus.Publish(EventBus.OnDragStarted);

                var dragData = new DataObject(string.Empty);
                DragDrop.DoDragDrop(Host.DraggingElement, dragData, DragDropEffects.Copy);
            }
        }

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Host.DraggingElementStartPoint = e.GetPosition(null);
            Host.DraggingElement           = (UIElement) sender;
            Host.LastSelectedUIElement = (UIElement) sender;
            EventBus.Publish(EventBus.OnDragElementSelected);
        }

        void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Host.DraggingElement = null;
            EventBus.Publish(EventBus.OnAfterDropOperation);
        }
        #endregion
    }
}