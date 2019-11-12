using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ___Company___.EntityGeneration.DataFlow.DataContext;
using ___Company___.EntityGeneration.DataFlow;

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
                Context.Get(Data.Config).BuildAfterCodeGenerationIsCompleted = false;

                BOACardDatabaseExporter.Export(kernel, "CRD");
            }
        }
        #endregion
    }
}