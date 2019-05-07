using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
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
                BOACardDatabaseExporter.Export(kernel, "PRM");
            }
        }
        #endregion

        class TestKernel : Kernel
        {
        }
    }
}