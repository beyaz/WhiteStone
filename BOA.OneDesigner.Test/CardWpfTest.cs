using System.Collections.Generic;
using System.Windows;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.MainForm;
using BOA.OneDesigner.WpfControls;
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

            Assert.AreEqual("A", bDataGridColumnWpf?._label.Text);

            bDataGrid.Columns[0].Label.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            bDataGridColumnWpf = bDataGridInfoWpf.ColumnsCollection[0] as BDataGridColumnWpf;
            Assert.AreEqual("B", bDataGridColumnWpf?._label.Text);
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

            Assert.AreEqual("?", cardWpf._groupBox.Header as string);

            bCard.TitleInfo.FreeTextValue = "B";

            host.EventBus.Publish(EventBus.OnComponentPropertyChanged);

            Assert.AreEqual("B", cardWpf._groupBox.Header as string);
        }
        #endregion
    }
}