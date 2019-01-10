using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class CardWpfTest
    {
        #region Public Methods
        [TestMethod]
        public void DeAttachToEventBus_should_clear_EventBus()
        {
            var host = new Host();

            var bCard = TestData.CreateBCardWithTwoInput();

            var wpf = host.Create<BCardWpf>(bCard);

            wpf.AttachToEventBus();

            DoSomeInteractions(wpf);

            wpf.DeAttachToEventBus();

            host.EventBus.CountOfListeningEventNames.Should().Be(0);
        }

        static void DoSomeInteractions(BCardWpf wpf)
        {
            wpf.Refresh();
            wpf.EnterDropLocationMode();
            wpf.ExitDropLocationMode();
            wpf.Refresh();
            wpf.Refresh();
            wpf.Refresh();
        }

        [TestMethod]
        public void Should_remove_when_item_deleted()
        {
            var host = new Host();

            var bCard = TestData.CreateBCardWithTwoInput();

            var wpf = host.Create<BCardWpf>(bCard);
            wpf.AttachToEventBus();

            DoSomeInteractions(wpf);

            wpf.BChildrenCount.Should().Be(2);
            var childAt_0 = wpf.BChildAt(0);

            // ACT
            bCard.Items.RemoveAt(1);
            host.EventBus.Publish(EventBus.ComponentDeleted);

            // ASSERT
            wpf.BChildrenCount.Should().Be(1);

            wpf.BChildAt(0).Should().NotBeSameAs(childAt_0);
        }

        [TestMethod]
        public void Should_update_when_card_header_property_changed()
        {
            var host = new Host();

            var bCard = new BCard
            {
                TitleInfo = LabelInfoHelper.CreateNewLabelInfo("?")
            };

            var wpf = host.Create<BCardWpf>(bCard);

            wpf.AttachToEventBus();

            wpf.HeaderAsString.Should().Be("?");

            // ACT
            bCard.TitleInfo.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            // ASSERT
            wpf.HeaderAsString.Should().Be("B");
        }
        #endregion
    }
}