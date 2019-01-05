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
        public void Should_remove_when_item_property_changed()
        {
            var host = new Host();

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
        public void Should_update_data_grid_column_when_property_changed()
        {
            var host = new Host();

            var bDataGrid = new BDataGrid
            {
                Columns = new List<BDataGridColumnInfo>
                {
                    new BDataGridColumnInfo
                    {
                        Label = new LabelInfo
                        {
                            FreeTextValue = "A",
                            IsFreeText    = true
                        }
                    }
                }
            };

            var bDataGridInfoWpf = host.Create<BDataGridInfoWpf>(bDataGrid);

            bDataGridInfoWpf.RaiseLoadedEvent();

            var bDataGridColumnWpf = (BDataGridColumnWpf) bDataGridInfoWpf.ColumnsCollection[0];

            bDataGridColumnWpf._label.Text.Should().Be("A");

            bDataGrid.Columns[0].Label.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            bDataGridColumnWpf = (BDataGridColumnWpf) bDataGridInfoWpf.ColumnsCollection[0];

            bDataGridColumnWpf._label.Text.Should().Be("B");
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
}