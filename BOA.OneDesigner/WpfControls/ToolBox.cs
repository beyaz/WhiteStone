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

           
            

            Loaded   += (s, e) => { AttachToEventBus(); };
            Unloaded += (s, e) => { DeAttachToEventBus(); };

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion


        public void AttachToEventBus()
        {
            Host.EventBus.Subscribe(EventBus.OnAfterDropOperation, Refresh);
        }
        public void DeAttachToEventBus()
        {
            Host.EventBus.UnSubscribe(EventBus.OnAfterDropOperation, Refresh);
        }


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
                    TitleInfo = LabelInfoHelper.CreateNewLabelInfo("? Title ?")
                }
            };

            Host.DragHelper.MakeDraggable(bCard);

            stackPanel.Children.Add(bCard);

            var bInput = new BInputWpf
            {
                Host        = Host,
                DataContext = new BInput {LabelInfo = LabelInfoHelper.CreateNewLabelInfo(), BindingPath = "?"}
            };

            Host.DragHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);

            var dataGridInfoWpf = new BDataGridInfoWpf
            {
                IsInToolbox = true,
                Host = Host,
                DataContext = new BDataGrid
                {
                    Columns = new List<BDataGridColumnInfo>
                    {
                        new BDataGridColumnInfo
                        {
                            Label = LabelInfoHelper.CreateNewLabelInfo()
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
        
        #endregion
    }
}