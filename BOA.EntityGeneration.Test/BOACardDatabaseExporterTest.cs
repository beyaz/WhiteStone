using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ___Company___.EntityGeneration.DataFlow.DataContext;
using ___Company___.EntityGeneration.DataFlow;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        ////[TestMethod]
        //public void Export()
        //{
        //    using (var kernel = new Kernel())
        //    {
        //        BOACardDatabaseExporter.Export(kernel);
        //    }
        //}

        [TestMethod]
        public void ExportPRM()
        {
            var context = Kernel.CreateDataContext(null,false,null);

            context.Get(Data.Config).BuildAfterCodeGenerationIsCompleted = false;

            BOACardDatabaseExporter.Export(context, "CRD");
        }
        #endregion
    }
}