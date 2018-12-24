using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BOA.OneDesigner.JsxElementRender;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;
using  BOA.OneDesigner.WpfControls;
using BCard = BOA.OneDesigner.WpfControls.BCard;
using BInput = BOA.OneDesigner.WpfControls.BInput;

namespace BOA.OneDesigner.DragAndDrop
{

   

   

    static class Visualizer
    {
        #region Public Methods
        public static UIElement Visualize(JsxElementModel.BInput bInput)
        {
            return new BInput
            {
                DataContext = bInput
            };
        }

        public static UIElement Visualize(JsxElementModel.BCard bCard)
        {
            var inWpf = new BCard
            {
                DataContext = bCard
            };



            

            Helper.MakeDraggable(inWpf);
            // Helper.MakeDropLocation(inWpf);

            return inWpf;
        }
        #endregion
    }

    

    static class Helper
    {

        public static void MakeDraggable(UIElement element)
        {

            element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;

            element.PreviewMouseMove += OnMouseMove;
        }

        #region Public Methods
        public static void MakeDropLocation(UIElement element)
        {
            element.AllowDrop =  true;
            element.Drop      += OnDrop;

            element.DragEnter += OnDragEnter;
            element.DragLeave += OnDragLeave;
        }
        #endregion

        #region Methods

        static void OnDragEnter(object sender , DragEventArgs e)
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
            if (e.Data.GetDataPresent("myFormat"))
            {
                var dropLocation = sender as DropLocation;
                var bInput = Info.Current?.Sender as BInput;

                if (bInput != null && dropLocation != null)
                {
                    dropLocation.OnDropAction(dropLocation);
                }
                
                
                
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

            if (
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var bCardInWpf = sender as BCard;
                if (bCardInWpf != null)
                {

                    bCardInWpf.EnterDropLocationMode();

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
                Sender =(UIElement) sender
            };
        }
        #endregion
    }

    public class Info
    {
        #region Static Fields
        public static Info Current;
        #endregion

        #region Fields
        public Point StartPoint;
        #endregion

        public UIElement Sender { get; set; }
    }
}