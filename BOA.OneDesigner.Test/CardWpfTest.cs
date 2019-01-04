using System.Collections.Generic;
using System.Windows;
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
        public void BDataGrid()
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

            bDataGridInfoWpf.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            var bDataGridColumnWpf = bDataGridInfoWpf.ColumnsCollection[0] as BDataGridColumnWpf;

            bDataGridColumnWpf?._label.Text.Should().Be("A");

            bDataGrid.Columns[0].Label.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            bDataGridColumnWpf = bDataGridInfoWpf.ColumnsCollection[0] as BDataGridColumnWpf;
            bDataGridColumnWpf?._label.Text.Should().Be("B");
        }

        [TestMethod]
        public void Header()
        {
            var host = new Host();

            var bCard = new BCard
            {
                TitleInfo = LabelInfoHelper.CreateNewLabelInfo("?")
            };

            var cardWpf = host.Create<BCardWpf>(bCard);
            cardWpf.AttachToEventBus();

            (cardWpf._groupBox.Header as string).Should().Be("?");

            bCard.TitleInfo.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            (cardWpf._groupBox.Header as string).Should().Be("B");
        }
        #endregion
    }
}