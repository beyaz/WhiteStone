using BOA.CodeGeneration.Contracts.Transforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    [TestClass]
    public class ExporterTest
    {
        [TestMethod]
        public void ExportSchema()
        {
            using (var database = new TestDatabase())
            {
                Exporter.ExportSchema(database, TestDatabase.CatalogName, "TST", data => { data.DatabaseEnumName = "BOACard"; });
            }
        }
    }
}