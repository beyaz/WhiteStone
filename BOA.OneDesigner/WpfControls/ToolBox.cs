using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.PropertyEditors;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class ToolBox : GroupBox, IHostItem
    {
        #region Constructors
        public ToolBox()
        {
            Header = "Tool Box";

            Loaded += (s, e) => { Refresh(); };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Methods
        void Refresh()
        {
            (Content as StackPanel)?.Children.RemoveAll();

            var stackPanel = new StackPanel();

            #region bInput
            var bInput = new BInputWpf
            {
                IsInToolbox = true,
                Host        = Host,
                DataContext = new BInput {LabelInfo = LabelInfoHelper.CreateNewLabelInfo("Input"), BindingPath = "?"}
            };

            Host.DragHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);
            #endregion


            #region ComboBox
            var bComboBoxInWpf = new BComboBoxInWpf
            {
                IsInToolbox = true,
                Host        = Host,
                DataContext = new BComboBox {LabelInfo = LabelInfoHelper.CreateNewLabelInfo("Combo"), BindingPath = "?"}
            };

            Host.DragHelper.MakeDraggable(bComboBoxInWpf);

            stackPanel.Children.Add(bComboBoxInWpf);
            #endregion


            #region bCard
            var bCard = new BCardWpf
            {
                Margin      = new Thickness(0, 10, 0, 0),
                Host        = Host,
                Height      = 100,
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
                Margin      = new Thickness(0, 10, 0, 0),
                IsInToolbox = true,
                Host        = Host,
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
            var tabControlWpf = new BTabBarWpf
            {
                Margin      = new Thickness(0, 10, 0, 0),
                Host        = Host,
                Height      = 100,
                IsInToolbox = true,
                DataContext = new BTabBar()
            };
            tabControlWpf.Refresh();

            Host.DragHelper.MakeDraggable(tabControlWpf);

            stackPanel.Children.Add(tabControlWpf);
            #endregion

            Content = stackPanel;
        }
        #endregion
    }
}