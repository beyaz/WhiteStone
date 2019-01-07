using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class DropLocation : Border,IHostItem
    {
        #region Constructors
        public DropLocation()
        {
            BorderBrush     = Brushes.HotPink;
            BorderThickness = new Thickness(10);
            MinHeight       = 15;
            MinWidth        = 20;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;

            Loaded += (s, e) => { new DropLocationHelper(Host).MakeDropLocation(this); };

            OnDragLeave();
        }
        #endregion

        #region Public Properties
        public Action<DropLocation> OnDropAction        { get; set; }
        public int                  TargetLocationIndex { get; set; }
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

       

        public Host Host { get; set; }
    }


    class DropLocationHelper
    {
        public Host Host { get; set; }
        public DropLocationHelper(Host host)
        {
            Host = host;
        }

        #region Public Methods
        public  void MakeDropLocation(UIElement element)
        {
            element.AllowDrop = true;

            element.Drop += OnDrop;

            element.DragEnter += OnDragEnter;

            element.DragLeave += OnDragLeave;
        }
        #endregion

        #region Methods
         void OnDragEnter(object sender, DragEventArgs e)
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

         void OnDragLeave(object sender, DragEventArgs e)
        {
            (sender as DropLocation)?.OnDragLeave();
        }

         void OnDrop(object sender, DragEventArgs e)
        {
            var dropLocation = sender as DropLocation;

            dropLocation?.OnDropAction(dropLocation);

            Host.EventBus.Publish(EventBus.OnAfterDropOperation);
        }
        #endregion
    }
}