using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BOA.OneDesigner.JsxElementRender;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.DragAndDrop
{

    static class Visualizer
    {
        public static UIElement Visualize(BInput bInput)
        {
            return new LabeledTextBox{DataContext = bInput}.LoadJson("{Label:'{Binding " + nameof(BInput.Label) + "}'}");
        }

        public static UIElement Visualize(BCard bCard)
        {
            var groupBox = new GroupBox().LoadJson("{Header:'{Binding "+nameof(BCard.Title)+"}'}");

            groupBox.DataContext = bCard;

            var sp = new StackPanel();

            groupBox.Content = sp;

            foreach (var bField in bCard.Fields)
            {
                if (bField is BInput)
                {
                    sp.Children.Add(new YoungBird{ Height = 5});
                    sp.Children.Add(Visualize((BInput) bField));
                    sp.Children.Add(new YoungBird{ Height = 5});
                    continue;
                }

            }

            return groupBox;

        }
        
    }


    public class  YoungBird:Border
    {
        public YoungBird()
        {
            BorderBrush = Brushes.HotPink;
            BorderThickness = new Thickness(2);

            MouseEnter += YoungBird_MouseEnter
                ;
            
        }

        private void YoungBird_MouseEnter(object sender, MouseEventArgs e)
        {
            this.BorderBrush = Brushes.Blue;
            BorderThickness = new Thickness(6);
            Height += 20;
        }
    }

    static class Helper
    {

        
        public static void OnDragEnter(object sender, DragEventArgs e)
        {
            var youngBird = sender as YoungBird;

            if (youngBird  == null)
            {
                return;
            }



            if (!e.Data.GetDataPresent("myFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
        }


        public static void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                var        contact = e.Data.GetData("myFormat") as string;
                StackPanel sp      = sender as StackPanel;


                sp.Children.Add(new Button{Content = contact});




            }
        }

        public static void  DropList_OnDragLeave(object sender, DragEventArgs e)
        {
            
            var stackPanel = sender as StackPanel;

            if (stackPanel != null)
            {
                stackPanel.Background = Brushes.LightBlue;
                stackPanel.Height     = 100;
            }
        }

        static void OnMouseMove(object sender, MouseEventArgs e)
        {
            var info = Info.Current;
            if (info == null)
            {
                return;
            }

            // Get the current mouse position
            Point  mousePos = e.GetPosition(null);
            Vector diff     = info.StartPoint - mousePos;
 
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance )
            {
                // Get the dragged ListViewItem
                StackPanel listView = sender as StackPanel;
                Button listViewItem = 
                    FindAnchestor<Button>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                {
                    return;
                }
                // Find the data behind the ListViewItem
                
 
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", "A" );
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy);
            } 
        }


         static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if( current is T )
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Info.Current = new Info {StartPoint = e.GetPosition(null)};
        }
    }
    public class Info
    {
        public static Info Current;
        public Point StartPoint;
    }
}
