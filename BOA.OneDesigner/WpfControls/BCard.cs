using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.DragAndDrop;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BCard : Grid
    {

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

        JsxElementModel.BCard Data => (JsxElementModel.BCard)DataContext;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

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
        }
        #endregion

        #region Public Properties
        public bool IsEnteredDropLocationMode { get; set; }
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

        public void OnDrop(DropLocation dropLocation)
        {
            var index = ChildrenContainer.Children.IndexOf(dropLocation);
            if (index<0)
            {
                throw new ArgumentException();
            }

            var bInput = Info.Current.Sender as BInput;
            if (bInput != null)
            {
                Data.Fields.Insert(index,bInput.Data);
                var dataContext = DataContext;
                DataContext = null;
                DataContext = dataContext;

                return;
            }

            throw new ArgumentException();
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
        #endregion
    }
}