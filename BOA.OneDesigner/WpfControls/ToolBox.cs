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


            #region bInput
            var bInput = new BInputWpf
            {
                Host        = Host,
                DataContext = new BInput { LabelInfo = LabelInfoHelper.CreateNewLabelInfo("Input"), BindingPath = "?" }
            };

            Host.DragHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);
            #endregion

            #region bCard
            var bCard = new BCardWpf
            {
                Host = Host,
                Height = 100,
                IsInToolbox = true,
                DataContext = new BCard
                {
                    TitleInfo = LabelInfoHelper.CreateNewLabelInfo("Card")
                }
            };

            Host.DragHelper.MakeDraggable(bCard);

            stackPanel.Children.Add(bCard); 
            #endregion

            

            #region BDataGrid
            var dataGridInfoWpf = new BDataGridInfoWpf
            {

                IsInToolbox = true,
                Host = Host,
                DataContext = new BDataGrid
                {
                    DataSourceBindingPath = "Data Grid",
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
            #endregion


            #region TabControl
            var tabControlWpf = new BTabBarWpf()
            {
                Host        = Host,
                Height      = 100,
                IsInToolbox = true,
                DataContext = new JsxElementModel.BTabBar
                {
                    Items = new List<TabPage>
                    {
                        new TabPage
                        {
                            TitleInfo = LabelInfoHelper.CreateNewLabelInfo("Tab Page Header")
                        }
                    }
                }
            };

            Host.DragHelper.MakeDraggable(tabControlWpf);

            stackPanel.Children.Add(tabControlWpf); 
            #endregion

            Content = stackPanel;
        }
        #endregion

        #region Methods
        
        #endregion
    }
}