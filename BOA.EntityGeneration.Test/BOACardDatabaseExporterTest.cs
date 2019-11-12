using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        //[TestMethod]
        public void Export()
        {
            using (var kernel = new Kernel())
            {
                BOACardDatabaseExporter.Export(kernel);
            }
        }

        [TestMethod]
        public void ExportPRM()
        {
            using (var kernel = new Kernel())
            {
                kernel.Get<Config>().BuildAfterCodeGenerationIsCompleted = false;

                BOACardDatabaseExporter.Export(kernel, "CRD");
            }
        }
        #endregion
    }
}