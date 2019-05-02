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
            using (var kernel = new TestKernel())
            {
                BOACardDatabaseExporter.Export(kernel);
            }
        }

        [TestMethod]
        public void ExportPRM()
        {
            using (var kernel = new TestKernel())
            {
                BOACardDatabaseExporter.Export(kernel, "CRD");
            }
        }
        #endregion
    }

    class TestKernel : Kernel
    {
    }
}