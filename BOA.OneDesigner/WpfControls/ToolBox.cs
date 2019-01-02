using System.Collections.Generic;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class ToolBox : GroupBox, IHostItem
    {
        #region Constructors
        public ToolBox()
        {
            Header = "Tool Box";

            EventBus2.Subscribe(EventBus2.OnAfterDropOperation, Refresh);

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            (Content as StackPanel)?.Children.RemoveAll();

            var stackPanel = new StackPanel();

            var bCard = new BCardWpf
            {
                Host        = Host,
                Height      = 100,
                IsInToolbox = true,
                DataContext = new BCard
                {
                    TitleInfo = CreateDefaultLabelInfo("Title")
                }
            };

            Host.DragHelper.MakeDraggable(bCard);

            stackPanel.Children.Add(bCard);

            var bInput = new BInputWpf
            {
                Host        = Host,
                DataContext = new BInput {LabelInfo = CreateDefaultLabelInfo(), BindingPath = "?"}
            };

            Host.DragHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);

            var dataGridInfoWpf = new BDataGridInfoWpf
            {
                Host = Host,
                DataContext = new BDataGrid
                {
                    Columns = new List<BDataGridColumnInfo>
                    {
                        new BDataGridColumnInfo
                        {
                            Label = CreateDefaultLabelInfo()
                        }
                    }
                }
            };

            Host.DragHelper.MakeDraggable(dataGridInfoWpf);

            stackPanel.Children.Add(dataGridInfoWpf);

            Content = stackPanel;
        }
        #endregion

        #region Methods
        static LabelInfo CreateDefaultLabelInfo(string freeText = null)
        {
            return new LabelInfo
            {
                IsFreeText    = true,
                FreeTextValue = freeText ?? "Label",
                DesignerText  = "Label"
            };
        }
        #endregion
    }
}