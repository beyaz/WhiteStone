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
                foreach (var schemaName in SchemaInfoDao.GetAllUserCreatedSchemaNames(database))
                {
                    Exporter.ExportSchema(database, TestDatabase.CatalogName, schemaName, data => { data.DatabaseEnumName = "BOACard"; });    
                }

                
            }
        }
    }
}