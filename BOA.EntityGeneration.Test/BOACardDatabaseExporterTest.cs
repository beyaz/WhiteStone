using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void Export()
        {
            BOACardDatabaseExporter.Export();
        }
        #endregion
    }
}