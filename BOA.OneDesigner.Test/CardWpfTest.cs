using System.Collections.Generic;
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

        [TestMethod]
        public void Should_remove_when_item_property_changed()
        {
            var host = new Host();

            var bCard = TestData.CreateBCardWithTwoInput();

            var cardWpf = host.Create<BCardWpf>(bCard);

            cardWpf.RaiseLoadedEvent();
            cardWpf.BChildrenCount.Should().Be(2);

            // ACT
            bCard.Items.RemoveAt(1);
            host.EventBus.Publish(EventBus.ComponentDeleted);

            // ASSERT
            cardWpf.BChildrenCount.Should().Be(1);
        }

        [TestMethod]
        public void Should_update_when_card_header_property_changed()
        {
            var host = new Host();

            var bCard = new BCard
            {
                TitleInfo = LabelInfoHelper.CreateNewLabelInfo("?")
            };

            var cardWpf = host.Create<BCardWpf>(bCard);

            cardWpf.RaiseLoadedEvent();

            cardWpf._groupBox.Header.Should().Be("?");

            // ACT
            bCard.TitleInfo.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            // ASSERT
            cardWpf._groupBox.Header.Should().Be("B");
        }
        #endregion
    }

    static class TestData
    {
        #region Public Methods
        public static BCard CreateBCardWithTwoInput()
        {
            var bCard = new BCard
            {
                Items = new List<BField>
                {
                    new BInput
                    {
                        BindingPath = "A"
                    },
                    new BInput
                    {
                        BindingPath = "B"
                    }
                }
            };
            return bCard;
        }

        public static DivAsCardContainer CreateDivAsCardContainer()
        {
            return new DivAsCardContainer {Items = new List<BCard> {CreateBCardWithTwoInput(), CreateBCardWithTwoInput()}};
        }
        #endregion
    }
}