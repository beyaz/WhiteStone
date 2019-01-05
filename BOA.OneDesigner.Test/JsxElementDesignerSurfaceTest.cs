using BOA.OneDesigner.AppModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class JsxElementDesignerSurfaceTest
    {
        #region Public Methods
        [TestMethod]
        public void Can_drop_only_card_component()
        {
            var host = new Host();

            var surface = new JsxElementDesignerSurface {Host = host};

            surface.RaiseLoadedEvent();

            host.EventBus.Publish(EventBus.OnDragStarted);

            host.DraggingElement = new BInputWpf();

            // ACT
            host.EventBus.Publish(EventBus.OnDragStarted);

            surface.Children.Count.Should().Be(0);


            host.DraggingElement = new BCardWpf();

            // ACT
            host.EventBus.Publish(EventBus.OnDragStarted);

            surface.Children.Count.Should().Be(1);
        }
        #endregion
    }
}