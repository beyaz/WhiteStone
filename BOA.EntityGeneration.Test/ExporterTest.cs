using BOA.EntityGeneration.DbModelDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.SchemaToDllExporting
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
                "CIS",
                "CLR",
                "COR",
                "CRD",
                "DBT",
                "DLV",
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
                    SchemaExporter.Export(new SchemaExporterDataForBOACard
                    {
                        Database   = database,
                        SchemaName = schemaName
                    });
                }
            }
        }
        #endregion
    }
}