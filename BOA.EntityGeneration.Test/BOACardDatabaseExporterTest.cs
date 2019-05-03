using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        class TestKernel : Kernel
        {
        }

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

    
}