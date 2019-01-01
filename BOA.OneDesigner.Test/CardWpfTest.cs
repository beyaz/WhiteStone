using System.Collections.Generic;
using System.Windows;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class CardWpfTest
    {
        #region Public Methods
        [TestMethod]
        public void Header()
        {
            var bCard = new BCard
            {
                TitleInfo = new LabelInfo
                {
                    FreeTextValue = "A",
                    IsFreeText    = true
                }
            };

            var cardWpf = new BCardWpf
            {
                DataContext = bCard
            };

            Assert.AreEqual("A", cardWpf._groupBox.Header as string);

            bCard.TitleInfo.FreeTextValue = "B";

            EventBus2.Publish(EventBus2.OnComponentPropertyChanged);

            Assert.AreEqual("B", cardWpf._groupBox.Header as string);
        }


        [TestMethod]
        public void BDataGrid()
        {
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

            var bDataGridInfoWpf = new BDataGridInfoWpf
            {
                DataContext = bDataGrid
            };
            bDataGridInfoWpf.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            var bDataGridColumnWpf = bDataGridInfoWpf.ColumnsCollection[0] as BDataGridColumnWpf;

            Assert.AreEqual("A", bDataGridColumnWpf?._label.Text);

            bDataGrid.Columns[0].Label.FreeTextValue = "B";

            EventBus2.Publish(EventBus2.OnComponentPropertyChanged);

            bDataGridColumnWpf = bDataGridInfoWpf.ColumnsCollection[0] as BDataGridColumnWpf;
            Assert.AreEqual("B", bDataGridColumnWpf?._label.Text);
        }

        #endregion
    }
}