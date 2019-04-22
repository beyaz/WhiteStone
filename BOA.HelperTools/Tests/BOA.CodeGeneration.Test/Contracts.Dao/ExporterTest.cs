using BOA.CodeGeneration.Contracts.Transforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    [TestClass]
    public class ExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void ExportBOACardDatabase()
        {
            var schemaNames = new[]
            {
                //"BKM",
                //"BNS",
                //"CAP",
                //"CCA",
                //"CFG",
                //"CGEN",
                //"CIS",
                //"CLR",
                //"COR",
                //"CRD",
                //"DBT",
                //"DLV",
                //"EMB",
                //"EMC",
                //"EMV",
                //"ESW",
                //"FRD",
                //"KMT",
                //"LOG",
                "MRC",
                "ORC",
                "POS",
                "PPD",
                "PRM",
                "RKL",
                "SYSOP",
                "STM",
                "SWC",
                "TMP",
                "TMS",
                "TRN",
                "VIS"
            };

            using (var database = new BOACardDatabase())
            {
                foreach (var schemaName in schemaNames)
                {
                    Exporter.ExportSchema(database, "BOACard", schemaName, null);
                }
            }
        }

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
        #endregion
    }
}