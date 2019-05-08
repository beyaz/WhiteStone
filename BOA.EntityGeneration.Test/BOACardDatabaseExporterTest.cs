using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

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
        public void ExportAllBatFiles()
        {
            using (var kernel = new TestKernel())
            {
                kernel.Get<BatExporter>().ExportAllInOne();

                foreach (var schemaName in kernel.Get<BOACardDatabaseSchemaNames>().SchemaNames)
                {
                    kernel.Get<BatExporter>().Export(schemaName);
                }
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

        class TestKernel : Kernel
        {
        }
    }
}