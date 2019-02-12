using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility.TypescriptModelGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.ExportingModel
{
    [TestClass]
    public class ExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void GenerateType()
        {
            var path   = @"D:\work\BOA.BusinessModules\Dev\BOA.CardGeneral.DebitCard\BOA.CardGeneral.DebitCard.sln";

            BOAPlugins.TypescriptModelGeneration.Handler.Handle(path);

        }

        [TestMethod]
        public void GetTypeNameInScope()
        {
            Assert.AreEqual("Types.Aloha", Exporter.GetTypeNameInContainerNamespace("BOA.Types.Aloha", "BOA"));
            Assert.AreEqual("OP.Aloha", Exporter.GetTypeNameInContainerNamespace("BOA.Types.OP.Aloha", "BOA.Types.UI"));
        }
        #endregion
    }
}