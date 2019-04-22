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
                "BKM",
                "BNS",
                "CAP",
                "CCA",
                "CFG",
                "CGEN",
                "CIS",
                "CLR",
                "COR",
                "CRD",
                "DBT",
                "DLV",
                "EMB",
                "EMC",
                "EMV",
                "ESW",
                "FRD",
                "KMT",
                "LOG",
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
                    SchemaExporter.Export(new SchemaExporterData
                    {
                        Database           = database,
                        CatalogName        = "BOACard",
                        SchemaName         = schemaName,
                        TableNameFilter = (tableName) => IsReadyToExport(schemaName,tableName)
                    });
                }
            }

        }

        static bool IsReadyToExport(string schemaName,string tableName)
        {
            
            if ($"{schemaName}.{tableName}" == "MRC.SENDING_REPORT_PARAMS_")
            {
                return false;
            }

            if ($"{schemaName}.{tableName}" == "POS.POS_INVENTORY_")
            {
                return false;
            }
            if ($"{schemaName}.{tableName}" == "STM.BUCKET_CLOSE_DETAIL") // todo aynı indexden iki tane var ?  sormak lazım
            {
                return false;
            }

            return true;
        }

        [TestMethod]
        public void ExportSchema()
        {
            using (var database = new TestDatabase())
            {
                foreach (var schemaName in SchemaInfoDao.GetAllUserCreatedSchemaNames(database))
                {
                    
                    SchemaExporter.Export(new SchemaExporterData
                    {
                        Database           = database,
                        CatalogName        = TestDatabase.CatalogName,
                        SchemaName         = schemaName,
                        OnTableDataCreated = data => { data.DatabaseEnumName = "BOACard"; }
                    });
                }
            }
        }
        #endregion
    }
}