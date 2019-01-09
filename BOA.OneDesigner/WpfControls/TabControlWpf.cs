using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public sealed class BTabBarWpf : TabControl, IHostItem, ISupportSizeInfo
    {
        #region Constructors
        public BTabBarWpf()
        {
            var tabItem = new TabItem
            {
                Header = "Tab Page 1"
            };
            AddChild(tabItem);
        }
        #endregion

        #region Public Properties
        public Host    Host        { get; set; }
        public bool    IsInToolbox { get; set; }
        public BTabBar Model       => (BTabBar) DataContext;

        public SizeInfo SizeInfo { get; } = new SizeInfo {IsMedium = true};
        #endregion
    }
}