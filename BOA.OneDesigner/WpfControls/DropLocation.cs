using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class DropLocation : Border
    {

        

        static class Helper
        {
            public static void MakeDropLocation(UIElement element)
            {
                element.AllowDrop = true;

                element.Drop += OnDrop;

                element.DragEnter += OnDragEnter;

                element.DragLeave += OnDragLeave;
            }

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
                var dropLocation = sender as DropLocation;

                dropLocation?.OnDropAction(dropLocation);

                EventBus.Publish(EventBus.OnAfterDropOperation);

            }
        }


        #region Constructors
        public DropLocation()
        {
            BorderBrush     = Brushes.HotPink;
            BorderThickness = new Thickness(10);
            MinHeight       = 20;
            MinWidth        = 20;

            Helper.MakeDropLocation(this);

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