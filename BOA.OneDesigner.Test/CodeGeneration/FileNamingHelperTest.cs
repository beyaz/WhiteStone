using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class FileNamingHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void OutputTypeScriptFileName_should_be_evaluated_by_tire()
        {
            var data = new ScreenInfo()
            {
                RequestName = "A.Bbb.UserIdRequest"
            };

            FileNamingHelper.InitDefaultOutputTypeScriptFileName(data);

            data.OutputTypeScriptFileName.Should().Be("user-id");
            
        }
        #endregion
    }
}