using System.Collections.Generic;
using System.Windows;
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
                DataContext = new BInput {LabelInfo = LabelInfoHelper.CreateNewLabelInfo("Input"), ValueBindingPath = "?"}
            };

            Host.DragHelper.MakeDraggable(bInput);

            stackPanel.Children.Add(bInput);
            #endregion

            #region ComboBox
            var bComboBoxInWpf = new BComboBoxInWpf
            {
                IsInToolbox = true,
                Host        = Host,
                DataContext = new BComboBox {LabelInfo = LabelInfoHelper.CreateNewLabelInfo("Combo"), ValueBindingPath = "?"}
            };

            Host.DragHelper.MakeDraggable(bComboBoxInWpf);

            stackPanel.Children.Add(bComboBoxInWpf);
            #endregion

            #region Label
            var bLabelInWpf = new BLabelWpf
            {
                Text        = "Label",
                FontWeight  = FontWeight.FromOpenTypeWeight(700),
                Host        = Host,
                DataContext = new BLabel(),
                IsInToolbox = true
            };

            Host.DragHelper.MakeDraggable(bLabelInWpf);

            stackPanel.Children.Add(bLabelInWpf);
            #endregion

            #region divider
            var divider = ComponentWpf.Create(Host, new ComponentInfo
            {
                Type = new ComponentType
                {
                    IsDivider = true
                }
            });
            divider.Model.IsInToolbox = true;

            Host.DragHelper.MakeDraggable(divider);

            stackPanel.Children.Add(divider);
            #endregion

            #region branchComponent
            var branchComponent = ComponentWpf.Create(Host, new ComponentInfo
            {
                Type = new ComponentType
                {
                    IsBranchComponent = true
                },
                LabelTextInfo = new LabelInfo
                {
                    FreeTextValue = "Branch Component",
                    IsFreeText    = true
                }
            });
            branchComponent.Model.IsInToolbox = true;

            Host.DragHelper.MakeDraggable(branchComponent);

            stackPanel.Children.Add(branchComponent);
            #endregion


            #region parameterComponent
            var parameterComponent = ComponentWpf.Create(Host, new ComponentInfo
            {
                Type = new ComponentType
                {
                    IsParameterComponent = true
                },
                LabelTextInfo = new LabelInfo
                {
                    FreeTextValue = "Parameter Component",
                    IsFreeText    = true
                }
            });
            parameterComponent.Model.IsInToolbox = true;

            Host.DragHelper.MakeDraggable(parameterComponent);

            stackPanel.Children.Add(parameterComponent);
            #endregion

            #region bCard
            var bCard = new BCardWpf
            {
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
                Host        = Host,
                Height      = 100,
                IsInToolbox = true,
                DataContext = new BTabBar()
            };
            tabControlWpf.Refresh();

            Host.DragHelper.MakeDraggable(tabControlWpf);

            stackPanel.Children.Add(tabControlWpf);
            #endregion


            foreach (var uiElement in stackPanel.Children.ToArray())
            {
                ((FrameworkElement)uiElement).Margin = new Thickness(0, 20, 0, 0);
            }

            Content = stackPanel;
        }
        #endregion
    }
}