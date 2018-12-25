using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class DropLocation : Border
    {
        #region Constructors
        public DropLocation()
        {
            BorderBrush     = Brushes.HotPink;
            BorderThickness = new Thickness(10);
            MinHeight       = 20;
            MinWidth        = 20;

            DragAndDropHelper.MakeDropLocation(this);

            OnDragLeave();
        }
        #endregion

        #region Public Properties
        public Action<DropLocation> OnDropAction        { get; set; }
        public int                   TargetLocationIndex { get; set; }
        #endregion

        #region Public Methods
        public void OnDragEnter()
        {
            BorderBrush = Brushes.BlueViolet;
        }

        public void OnDragLeave()
        {
            BorderBrush = Brushes.HotPink;
        }
        #endregion
    }
}