﻿using BOAPlugins.TypescriptModelGeneration;
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
            var path   = @"D:\work\BOA.BusinessModules\Dev\BOA.CardGeneral.DebitCard\One\BOA.One.CardGeneral.DebitCard\ClientApp\models\AutoGeneratedModelsConfig.json";
            var result = Exporter.Export(path);
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