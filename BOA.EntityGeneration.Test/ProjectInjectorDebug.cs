using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    [TestClass]
    public class CustomSqlExporterDebug
    {
        #region Public Methods
        [TestMethod]
        public void CustomSqlInjection()
        {
            var context = new DataContextCreator().Create();
            CustomSqlExporter.Export(context, "CC_OPERATIONS");
        }
        #endregion
    }
}