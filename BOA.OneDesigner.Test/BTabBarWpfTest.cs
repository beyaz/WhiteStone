using System.Windows;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class BTabBarWpfTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_open_tab_body_when_tab_header_clicked()
        {
            var host = new Host();

            var bTabBarWpf = host.CreateAndLoadBTabBarWpfWithTwoTab();

            bTabBarWpf.HeadersContainersWrapPanel.Children.Count.Should().Be(2);

            bTabBarWpf.HeadersContainersWrapPanel.Children[0].RaisePreviewMouseLeftButtonDownEvent();

            var tabBodies = bTabBarWpf.TabPageBodyList.Children;

            tabBodies.Count.Should().Be(2);

            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();

            tabBodies[0].Visibility.Should().Be(Visibility.Visible);
            tabBodies[1].Visibility.Should().Be(Visibility.Collapsed);
            

            // ((FrameworkElement)tabBodies[0]).Width.Should().BeGreaterThan(0);


        }
        #endregion
    }
}