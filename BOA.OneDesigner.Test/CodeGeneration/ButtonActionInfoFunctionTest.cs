using BOA.OneDesigner.CodeGenerationModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data = BOA.OneDesigner.CodeGeneration.ButtonActionInfoFunction;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class ButtonActionInfoFunctionTest
    {
        #region Public Methods
        [TestMethod]
        public void GetCode()
        {
            var api = new Data
            {
                ExtensionMethodName = "x",
                WriterContext       = new WriterContext()
            };

            api.GetCode().Trim().Should().BeEquivalentTo("Extension.x(this);");

            api = new Data
            {
                ExtensionMethodName = "x",
                WriterContext = new WriterContext
                {
                    IsTabPage = true
                }
            };

            api.GetCode().Should().Be("Extension.x(this.state.pageInstance);");
        }
        #endregion
    }
}