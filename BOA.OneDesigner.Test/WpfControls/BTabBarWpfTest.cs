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

            var wpf = host.CreateBTabBarWpfWithTwoTab();

            wpf.AttachToEventBus();

            DoSomeInteractions(wpf);

            wpf.TabCount.Should().Be(2);

            wpf.HeadersContainersWrapPanel.Children[0].RaisePreviewMouseLeftButtonDownEvent();

            var tabBodies = wpf.TabPageBodyList.Children;

            tabBodies.Count.Should().Be(2);

            DoSomeInteractions(wpf);

            tabBodies[0].Visibility.Should().Be(Visibility.Visible);
            tabBodies[1].Visibility.Should().Be(Visibility.Collapsed);

            host.SelectedElement = new BCardWpf();
            host.EventBus.Publish(EventBus.OnDragStarted);

            tabBodies[0].Visibility.Should().Be(Visibility.Visible);
            tabBodies[1].Visibility.Should().Be(Visibility.Collapsed);

            var tabPage0 = (DivAsCardContainerWpf) tabBodies[0];
            tabPage0.IsEnteredDropLocationMode.Should().BeTrue();

            wpf.DeAttachToEventBus();

            host.EventBus.CountOfListeningEventNames.Should().Be(0);
        }
        #endregion

        #region Methods
        static void DoSomeInteractions(BTabBarWpf wpf)
        {
            wpf.Refresh();
            wpf.Refresh();
            wpf.Refresh();
            wpf.Refresh();
        }
        #endregion
    }
}