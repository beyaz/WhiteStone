using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.MainForm;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class ViewTest
    {
        #region Public Methods
        [TestMethod]
        public void Refresh_method_should_transfer_jsxModel_between_Model_and_Surface()
        {
            var host = new Host();

            var model = new Model();

            var surface = new JsxElementDesignerSurface();

            View.Refresh(host, model, surface);

            surface.DataContext.Should().BeNull();

            model.ScreenInfo = new ScreenInfo();

            View.Refresh(host, model, surface);

            surface.DataContext.Should().BeNull();

            var jsxModel = new DivAsCardContainer();

            model.ScreenInfo.JsxModel = jsxModel;

            View.Refresh(host, model, surface);
            View.Refresh(host, model, surface);
            View.Refresh(host, model, surface);

            surface.DataContext.Should().Be(jsxModel);

            model.ScreenInfo.JsxModel = null;

            View.Refresh(host, model, surface);

            surface.DataContext.Should().Be(jsxModel);
            model.ScreenInfo.JsxModel.Should().Be(jsxModel);

            model.ScreenInfo.JsxModel = null;

            surface.DataContext = null;

            View.Refresh(host, model, surface);

            surface.DataContext.Should().BeNull();
            model.ScreenInfo.JsxModel.Should().BeNull();

            surface.DataContext = jsxModel;

            View.Refresh(host, model, surface);

            surface.DataContext.Should().Be(jsxModel);
            model.ScreenInfo.JsxModel.Should().Be(jsxModel);
        }
        #endregion
    }
}