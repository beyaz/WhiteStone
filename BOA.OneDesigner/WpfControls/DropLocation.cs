using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.DragAndDrop;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class DropLocation : Border
    {
        public Action<DropLocation> OnDropAction { get; }

        #region Constructors
        public DropLocation(Action<DropLocation> onDropAction)
        {
            OnDropAction = onDropAction;

            BorderBrush     = Brushes.HotPink;
            BorderThickness = new Thickness(10);
            MinHeight       = 20;
            MinWidth        = 20;

            Helper.MakeDropLocation(this);

            OnDragLeave();
        }
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