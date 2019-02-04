using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class RenderHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void Long_binding_paths_should_be_shorten_in_variables()
        {
            var writerContext = new WriterContext();

            var bindingPath = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, "dataContract.UserName");

            bindingPath.Should().Be("dataContract.userName");

            writerContext.RenderMethodRequestRelatedVariables.Should().BeSameAs(new[] {"const dataContract = request.dataContract;"});
        }
        #endregion
    }
}