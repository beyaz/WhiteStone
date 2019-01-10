using System.Windows;
using BOA.OneDesigner.AppModel;
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

            var bTabBarWpf = host.CreateBTabBarWpfWithTwoTab();
            bTabBarWpf.AttachToEventBus();
            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();

            bTabBarWpf.TabCount.Should().Be(2);

            bTabBarWpf.HeadersContainersWrapPanel.Children[0].RaisePreviewMouseLeftButtonDownEvent();

            var tabBodies = bTabBarWpf.TabPageBodyList.Children;

            tabBodies.Count.Should().Be(2);

            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();
            bTabBarWpf.Refresh();

            tabBodies[0].Visibility.Should().Be(Visibility.Visible);
            tabBodies[1].Visibility.Should().Be(Visibility.Collapsed);


            host.SelectedElement = new BCardWpf();
            host.EventBus.Publish(EventBus.OnDragStarted);


            tabBodies[0].Visibility.Should().Be(Visibility.Visible);
            tabBodies[1].Visibility.Should().Be(Visibility.Collapsed);

            var tabPage0 = (DivAsCardContainerWpf)tabBodies[0];
            tabPage0.IsEnteredDropLocationMode.Should().BeTrue();

            bTabBarWpf.DeAttachToEventBus();

            host.EventBus.CountOfListeningEventNames.Should().Be(0);

        }
        #endregion
    }
}