using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.WpfControls;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class DivAsCardContainerWpfTest
    {

        static void DoSomeInteractions(DivAsCardContainerWpf wpf)
        {
            wpf.Refresh();
            wpf.EnterDropLocationMode();
            wpf.ExitDropLocationMode();
            wpf.Refresh();
            wpf.Refresh();
            wpf.Refresh();
        }
        [TestMethod]
        public void DeAttachToEventBus_should_clear_EventBus()
        {
            var host = new Host();

            var wpf = host.Create<DivAsCardContainerWpf>(TestData.CreateDivAsCardContainer());


            wpf.AttachToEventBus();

            DoSomeInteractions(wpf);

            wpf.DeAttachToEventBus();

            host.EventBus.CountOfListeningEventNames.Should().Be(0);
        }


        [TestMethod]
        public void Other_cards_should_not_any_effect_when_any_input_deleted_in_another_card()
        {
            var host = new Host();

            var wpf = host.Create<DivAsCardContainerWpf>(TestData.CreateDivAsCardContainer());
            wpf.AttachToEventBus();

            DoSomeInteractions(wpf);

            var bChildAt_0 = wpf.BChildAt(0);

            wpf.Model.Items[1].Items.RemoveAt(0);
            host.EventBus.Publish(EventBus.ComponentDeleted);

            DoSomeInteractions(wpf);

            (wpf.BChildAt(0) == bChildAt_0).Should().BeTrue();
        }
    }
}