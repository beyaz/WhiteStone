using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class SampleDatabaseTest
    {
        #region Public Methods
        [TestMethod]
        public void AllInOne()
        {
            using (var kernel = new Kernel())
            {
                BOACardDatabaseExporter.Export(kernel);
            }
        }
        #endregion
    }
}