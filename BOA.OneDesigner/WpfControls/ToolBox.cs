using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class ToolBox : GroupBox
    {
        #region Constructors
        public ToolBox()
        {
            Header = "Tool Box";

            EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);

            Refresh();
        }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            var stackPanel = new StackPanel();

            var bCard = new BCardWpf
            {
                Height      = 100,
                IsInToolbox = true,
                DataContext = new BCard
                {
                   TitleInfo = CreateDefaultLabelInfo("Title")
                }
            };

            DragHelper.MakeDraggable(bCard);

            stackPanel.Children.Add(bCard);

            var bInput = new BInputWpf {DataContext = new BInput {LabelInfo = CreateDefaultLabelInfo(), BindingPath = "?"}};

            DragHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);

            Content = stackPanel;
        }
        #endregion



        static LabelInfo CreateDefaultLabelInfo(string freeText = null)
        {
            return new LabelInfo
            {
                IsFreeText    = true,
                FreeTextValue = freeText??"Label",
                DesignerText  = "Label"
            };
        }
    }
}