using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.DragAndDrop;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BCard : Grid, IDropLocationContainer
    {
        #region Fields
        public StackPanel ChildrenContainer;
        #endregion

        #region Constructors
        public BCard()
        {
            this.LoadJson(@"
{
	rows:
	[
		{
            view:   'GroupBox', 
            Header: '{Binding " + nameof(JsxElementModel.BCard.Title) + @"}',
            Content: { ui:'StackPanel' , Name:'" + nameof(ChildrenContainer) + @"' }
        }
	]
	
}");

            Helper.MakeDraggable(this);
        }
        #endregion

        #region Public Properties
        public bool IsEnteredDropLocationMode { get; set; }
        #endregion

        #region Properties
        JsxElementModel.BCard Data => (JsxElementModel.BCard) DataContext;
        #endregion

        #region Public Methods
        public void EnterDropLocationMode()
        {
            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

            var items = ChildrenContainer.Children.ToArray();

            ChildrenContainer.Children.Clear();

            foreach (var control in items)
            {
                ChildrenContainer.Children.Add(new DropLocation(OnDrop));

                ChildrenContainer.Children.Add(control);

                if (control == items[items.Length - 1])
                {
                    ChildrenContainer.Children.Add(new DropLocation(OnDrop));
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

            var items = ChildrenContainer.Children.ToArray();

            ChildrenContainer.Children.Clear();

            foreach (var control in items)
            {
                if (control is DropLocation)
                {
                    continue;
                }

                ChildrenContainer.Children.Add(control);
            }
        }

        public void OnDrop(IDropLocation dropLocation)
        {
         

            var dropLocationIndex = ChildrenContainer.Children.IndexOf((UIElement) dropLocation);
            if (dropLocationIndex < 0)
            {
                throw new ArgumentException();
            }

            UIElement previousElement = null;
            if (dropLocationIndex > 0)
            {
                previousElement = ChildrenContainer.Children[dropLocationIndex - 1];
            }
            else
            {
                previousElement = ChildrenContainer.Children[1];
            }

            ExitDropLocationMode();

            

            var previousComponentIndex = ChildrenContainer.Children.IndexOf(previousElement);
            if (previousComponentIndex < 0)
            {
                throw new ArgumentException();
            }


            var insertIndex = previousComponentIndex +1;

            if (dropLocationIndex == 0)
            {
                insertIndex = 0;
            }
            


            var bInput = Info.Current.Sender as BInput;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                Data.InsertField(insertIndex, bInput.Data);

                RefreshDataContext();

                return;
            }

            throw new ArgumentException();
        }

        public void Refresh()
        {
            ChildrenContainer.Children.Clear();

            if (Data == null)
            {
                return;
            }

            foreach (var bField in Data.Fields)
            {
                if (bField is JsxElementModel.BInput)
                {
                    var uiElement = new BInput
                    {
                        DataContext = bField
                    };

                    Helper.MakeDraggable(uiElement);

                    ChildrenContainer.Children.Add(uiElement);
                }
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