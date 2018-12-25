using System.Windows.Controls;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class ToolBox : GroupBox
    {
        #region Constructors
        public ToolBox()
        {
            Header = "Tool Box";

            DragAndDropHelper.AfterDropOperation += Refresh;

            Refresh();
        }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            var stackPanel = new StackPanel();

            var bCard = new BCard {Height = 100, DataContext = new JsxElementModel.BCard {Title = "Card"}};

            DragAndDropHelper.MakeDraggable(bCard);

            stackPanel.Children.Add(bCard);

            var bInput = new BInput {DataContext = new JsxElementModel.BInput {Label = "Label", BindingPath = "?"}};

            DragAndDropHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);

            Content = stackPanel;
        }
        #endregion
    }
}