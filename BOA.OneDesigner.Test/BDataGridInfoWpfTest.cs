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

            var dataGridInfoWpf = host.CreateAndLoadDataGridWpfWithTwoColumn();

            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(2);

            host.DraggingElement = new BDataGridColumnWpf(new BDataGridColumnInfo(), host, dataGridInfoWpf);

            host.EventBus.Publish(EventBus.OnDragStarted);

            // ASSERT
            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(2);
        }

        [TestMethod]
        public void Drop_locations_should_be_add_when_drag_started()
        {
            var host = new Host();

            var dataGridInfoWpf = host.CreateAndLoadDataGridWpfWithTwoColumn();

            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(2);

            host.DraggingElement = dataGridInfoWpf.ColumnsCollection[0];

            host.EventBus.Publish(EventBus.OnDragStarted);

            // ASSERT
            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(5);

            host.EventBus.Publish(EventBus.OnAfterDropOperation);

            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(2);
        }

        [TestMethod]
        public void Drop_locations_should_be_un_visible_when_any_own_column_removed()
        {
            var host = new Host();

            var dataGridInfoWpf = host.CreateAndLoadDataGridWpfWithTwoColumn();

            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(2);

            host.DraggingElement = new BDataGridColumnWpf(new BDataGridColumnInfo(), host, dataGridInfoWpf);

            host.EventBus.Publish(EventBus.DataGridColumnRemoved);

            // ASSERT
            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(2);
        }

        [TestMethod]
        public void Should_be_zero_column_when_in_toolbox()
        {
            var host = new Host();

            var dataGridInfoWpf = host.CreateDataGridWpfWithTwoColumn();
            dataGridInfoWpf.IsInToolbox = true;

            // ACT
            dataGridInfoWpf.RaiseLoadedEvent();

            // ASSERT
            dataGridInfoWpf.ColumnsCollection.Count.Should().Be(0);
        }
        #endregion
    }
}