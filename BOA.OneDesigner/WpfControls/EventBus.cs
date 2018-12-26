using System;

namespace BOA.OneDesigner.WpfControls
{
    public static class EventBus
    {
        public static event Action DragStarted;



        public static event Action AfterDropOperation;

        #region Public Events
        public static event Action DragElementSelected;
        #endregion

        #region Public Methods
        public static void OnDragElementSelected()
        {
            DragElementSelected?.Invoke();
        }
        #endregion

        public static void  OnAfterDropOperation()
        {
            AfterDropOperation?.Invoke();
        }

        public static void OnDragStarted()
        {
            DragStarted?.Invoke();
        }
    }
}