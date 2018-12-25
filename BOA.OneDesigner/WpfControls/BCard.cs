using System;
using System.Windows;
using System.Windows.Controls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BCard : Grid, IDropLocationContainer, IJsxElementDesignerSurfaceItem
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
        }
        #endregion

        #region Public Properties
        public IDropLocationContainer Container                 { get; set; }
        public JsxElementModel.BCard  Data                      => (JsxElementModel.BCard) DataContext;
        public bool                   IsEnteredDropLocationMode { get; set; }

        public JsxElementDesignerSurface Surface { get; set; }
        #endregion

        #region Public Methods
        public void EnterDropLocationMode()
        {
            if (!CanDrop(Surface.DraggingElement))
            {
                return;
            }

            if (IsEnteredDropLocationMode)
            {
                return;
            }

            IsEnteredDropLocationMode = true;

            var items = ChildrenContainer.Children.ToArray();

            ChildrenContainer.Children.Clear();

            for (var i = 0; i < items.Length; i++)
            {
                var control = items[i];

                var dropLocation = new DropLocation {OnDropAction = OnDrop, TargetLocationIndex = i};

                ChildrenContainer.Children.Add(dropLocation);

                ChildrenContainer.Children.Add(control);

            }

            ChildrenContainer.Children.Add(new DropLocation {OnDropAction = OnDrop, TargetLocationIndex = items.Length});
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

        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            Surface.ExitDropLocationMode();

            var bInput = Surface.DraggingElement as BInput;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();
                ((BCard) bInput.Container)?.RefreshDataContext();

                Data.InsertItem(insertIndex, bInput.Data);

                this.RefreshDataContext();

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

            foreach (var bField in Data.Items)
            {
                if (bField is JsxElementModel.BInput)
                {
                    var uiElement = new BInput
                    {
                        Surface     = Surface,
                        DataContext = bField,
                        Container   = this
                    };

                    DragAndDropHelper.MakeDraggable(uiElement);

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

        static bool CanDrop(UIElement dragElement)
        {
            if (dragElement is BInput)
            {
                return true;
            }

            return false;
        }

        
        #endregion
    }
}