using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.ExportingModel
{
    [TestClass]
    public class HandlerTest
    {
        #region Public Methods
        [TestMethod]
        public void Handle()
        {
            var path   = @"D:\work\BOA.BusinessModules\Dev\BOA.CardGeneral.DebitCard\BOA.CardGeneral.DebitCard.sln";
            var result = Handler.Handle(path);

            Assert.IsNull(result.ErrorMessage);

            var data = new ExportAsCSharpCodeData
            {
                solutionFilePath = path
            };

            MessagingExporter.ExportAsTypeScriptCode(data);
            Assert.IsNull(data.ErrorMessage);

            data = new ExportAsCSharpCodeData
            {
                solutionFilePath       = path,
                RemoveUnusedProperties = true
            };

            MessagingExporter.ExportAsCSharpCode(data);
            Assert.IsNull(data.ErrorMessage);
        }
        #endregion
    }
}