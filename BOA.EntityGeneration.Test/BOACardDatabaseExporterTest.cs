using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void CustomSqlInjection()
        {
            using (var kernel = new TestKernel())
            {
                kernel.Get<ProjectInjector>().Inject("CC_OPERATIONS");
            }
        }

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
            BOACardDatabaseExporter.Export("PRM");
        }
        #endregion

        class TestKernel : Kernel
        {
        }
    }
}