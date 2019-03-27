using System.Collections.Generic;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class DataGridWpfTest
    {
        #region Public Methods
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

            var wpf = host.CreateBDataGridInfoWpf(bDataGrid);

            wpf.AttachToEventBus();

            var bDataGridColumnWpf = (BDataGridColumnWpf) wpf.ColumnsCollection[0];

            bDataGridColumnWpf._label.Text.Should().Be("A");

            bDataGrid.Columns[0].Label.FreeTextValue = "B";

            host.SelectedElement = bDataGridColumnWpf;
            host.EventBus.Publish(EventBus.LabelChanged);

            bDataGridColumnWpf = (BDataGridColumnWpf) wpf.ColumnsCollection[0];

            bDataGridColumnWpf._label.Text.Should().Be("B");
        }
        #endregion
    }
}