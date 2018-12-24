using System.Windows;

namespace BOA.OneDesigner.DragAndDrop
{
    public class Info
    {
        #region Static Fields
        public static Info Current;
        #endregion

        #region Fields
        public Point StartPoint;
        #endregion

        #region Public Properties
        public UIElement Sender { get; set; }
        #endregion
    }
}