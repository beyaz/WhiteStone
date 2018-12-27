using System;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BCardWpf : Grid, IJsxElementDesignerSurfaceItem
    {
        #region Fields
        public StackPanel ChildrenContainer;
        #endregion

        #region Constructors
        public BCardWpf()
        {
            EventBus.DragStarted        += EnterDropLocationMode;
            EventBus.AfterDropOperation += ExitDropLocationMode;
            EventBus.AfterDropOperation += Refresh;

            this.LoadJson(@"
{
	rows:
	[
		{
            view:   'GroupBox', 
            Header: '{Binding " + nameof(BCard.Title) + @"}',
            Content: { ui:'StackPanel' , Name:'" + nameof(ChildrenContainer) + @"' }
        }
	]
	
}");
        }
        #endregion

        #region Public Properties
        public BCard Data                      => (BCard) DataContext;
        public bool  IsEnteredDropLocationMode { get; set; }

        public JsxElementDesignerSurface Surface { get; set; }
        #endregion

        #region Public Methods
        public void OnDrop(DropLocation dropLocation)
        {
            var insertIndex = dropLocation.TargetLocationIndex;

            var bInput = UIContext.DraggingElement as BInputWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

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
                if (bField is BInput)
                {
                    var uiElement = new BInputWpf
                    {
                        Surface     = Surface,
                        DataContext = bField
                        // Container   = this
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
            if (dragElement is BInputWpf)
            {
                return true;
            }

            return false;
        }


        public bool IsInToolbox { get; set; }

        void EnterDropLocationMode()
        {

            if (IsInToolbox)
            {
                return;
            }

            if (!CanDrop(UIContext.DraggingElement))
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

        void ExitDropLocationMode()
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