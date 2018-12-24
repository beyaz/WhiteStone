using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.DragAndDrop;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class DropLocation : Border,IDropLocation
    {
        public Action<IDropLocation> OnDropAction { get; }

        

        #region Constructors
        public DropLocation(Action<IDropLocation> onDropAction)
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