using BOA.OneDesigner.CodeGenerationHelper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class BindingPathHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void WindowRequestDefaultCreationAccordingToBindingPaths()
        {
            var input = new[]
            {
                "DataContract.User.NameInfo.Name",
                "DataContract.User.NameInfo2.Name"
            };
            var value = BindingPathHelper.EvaluateWindowRequestDefaultCreationInRenderFunction(input);
            value.Should().BeEquivalentTo("{ dataContract: { user: { nameInfo: {}, nameInfo2: {} } } }");
        }
        #endregion
    }
}