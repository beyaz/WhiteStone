using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.WpfControls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class DivAsCardContainerWpfTest
    {
        [TestMethod]
        public void DeAttachToEventBus_should_clear_EventBus()
        {
            var host = new Host();

            var wpf = host.Create<DivAsCardContainerWpf>(TestData.CreateDivAsCardContainer());


            wpf.AttachToEventBus();

            wpf.Refresh();
            wpf.EnterDropLocationMode();
            wpf.ExitDropLocationMode();
            wpf.Refresh();
            wpf.Refresh();
            wpf.Refresh();

            wpf.DeAttachToEventBus();

            host.DeAttachToEventBus();
            

            host.EventBus.CountOfListeningEventNames.Should().Be(0);
        }
    }
}