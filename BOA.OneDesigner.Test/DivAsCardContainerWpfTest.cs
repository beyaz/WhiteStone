using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.WpfControls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class DivAsCardContainerWpfTest
    {
        #region Public Methods
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
            var childCount = wpf.BChildCount;

            var bChildAt_0 = wpf.BChildAt(0);
            var bChildAt_1 = wpf.BChildAt(1);

            host.SelectedElement = wpf.BChildAt(1).BChildAt(0);

            var bChildCountOfCard = wpf.BChildAt(1).BChildrenCount;

            host.EventBus.Publish(EventBus.ComponentDeleted);

            (wpf.BChildAt(0) == bChildAt_0).Should().BeTrue();
            (wpf.BChildAt(1) == bChildAt_1).Should().BeTrue();

            wpf.BChildCount.Should().Be(childCount);
            wpf.BChildAt(1).BChildrenCount.Should().Be(bChildCountOfCard);
        }
        #endregion

        #region Methods
        static void DoSomeInteractions(DivAsCardContainerWpf wpf)
        {
            wpf.Refresh();
            wpf.EnterDropLocationMode();
            wpf.ExitDropLocationMode();
            wpf.Refresh();
            wpf.Refresh();
            wpf.Refresh();
        }
        #endregion
    }
}