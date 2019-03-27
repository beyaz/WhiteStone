using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class BDataGridInfoWpfTest
    {
        #region Public Methods
        [TestMethod]
        public void Can_drop_only_own_columns()
        {
            var host = new Host();

            var wpf = host.CreateAndLoadDataGridWpfWithTwoColumn();

            wpf.ColumnsCollection.Count.Should().Be(2);

            host.SelectedElement = new BDataGridColumnWpf(new BDataGridColumnInfo(), host, wpf);

            host.EventBus.Publish(EventBus.OnDragStarted);

            // ASSERT
            wpf.ColumnsCollection.Count.Should().Be(2);
        }

        [TestMethod]
        public void Drop_locations_should_be_add_when_drag_started()
        {
            var host = new Host();

            var wpf = host.CreateAndLoadDataGridWpfWithTwoColumn();
            wpf.AttachToEventBus();

            wpf.ColumnsCollection.Count.Should().Be(2);

            host.SelectedElement = wpf.ColumnsCollection[0];

            host.EventBus.Publish(EventBus.OnDragStarted);

            // ASSERT
            wpf.ColumnsCollection.Count.Should().Be(5);

            host.EventBus.Publish(EventBus.OnAfterDropOperation);

            wpf.ColumnsCollection.Count.Should().Be(2);
        }

        [TestMethod]
        public void Drop_locations_should_be_un_visible_when_any_own_column_removed()
        {
            var host = new Host();

            var wpf = host.CreateAndLoadDataGridWpfWithTwoColumn();

            wpf.ColumnsCollection.Count.Should().Be(2);

            host.SelectedElement = new BDataGridColumnWpf(new BDataGridColumnInfo(), host, wpf);

            host.EventBus.Publish(EventBus.DataGridColumnRemoved);

            // ASSERT
            wpf.ColumnsCollection.Count.Should().Be(2);
        }
        #endregion
    }
}