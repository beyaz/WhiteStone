using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.DragAndDrop;

namespace BOA.OneDesigner.WpfControls
{
    public class BCardSection : WrapPanel, IDropLocationContainer
    {
        
        #region Public Properties
        public bool IsEnteredDropLocationMode { get; set; }
        #endregion

        #region Properties
        JsxElementModel.BCardSection Data => (JsxElementModel.BCardSection) DataContext;
        #endregion

        #region Public Methods
        public void EnterDropLocationMode()
        {

            

            
            if (IsEnteredDropLocationMode)
            {
                return;
            }

            if (!(Info.Current?.Sender is BCard))
            {
                foreach (var child in Children)
                {
                    (child as IDropLocationContainer)?.EnterDropLocationMode();
                }

                return;
            }


            IsEnteredDropLocationMode = true;

            var items = Children.ToArray();

            Children.Clear();

            foreach (var control in items)
            {
                Children.Add(new DropLocation(OnDrop));

                (control as IDropLocationContainer)?.EnterDropLocationMode();

                Children.Add(control);

                if (control == items[items.Length - 1])
                {
                    Children.Add(new DropLocation(OnDrop));
                }
            }
        }

        public void ExitDropLocationMode()
        {
            if (!IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = false;


            if (!(Info.Current?.Sender is BCard))
            {
                foreach (var child in Children)
                {
                    (child as IDropLocationContainer)?.ExitDropLocationMode();
                }

                return;
            }


            var items = Children.ToArray();

            Children.Clear();

            foreach (var control in items)
            {
                if (control is DropLocation)
                {
                    continue;
                }

                (control as IDropLocationContainer)?.ExitDropLocationMode();

                Children.Add(control);
            }
        }

        public void OnDrop(IDropLocation dropLocation)
        {
            var dropLocationIndex = Children.IndexOf((UIElement) dropLocation);
            if (dropLocationIndex < 0)
            {
                throw new ArgumentException();
            }

            UIElement previousElement = null;
            if (dropLocationIndex > 0)
            {
                previousElement = Children[dropLocationIndex - 1];
            }
            else
            {
                previousElement = Children[1];
            }

            ExitDropLocationMode();

            var previousComponentIndex = Children.IndexOf(previousElement);
            if (previousComponentIndex < 0)
            {
                throw new ArgumentException();
            }

            var insertIndex = previousComponentIndex + 1;

            if (dropLocationIndex == 0)
            {
                insertIndex = 0;
            }

            var bInput = Info.Current.Sender as BCard;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                Data.InsertCard(insertIndex, bInput.Data);

                RefreshDataContext();

                return;
            }

            throw new ArgumentException();
        }

        public void Refresh()
        {
            Children.Clear();

            if (Data == null)
            {
                return;
            }

            foreach (var bField in Data.Cards)
            {
                var uiElement = new BCard
                {
                    DataContext = bField,
                    Margin = new Thickness(10),
                    Container =  this
                };

                Helper.MakeDraggable(uiElement);

                Children.Add(uiElement);
            }
        }
        #endregion

        #region Methods
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

        void RefreshDataContext()
        {
            var dataContext = DataContext;
            DataContext = null;
            DataContext = dataContext;
        }
        #endregion
    }
}