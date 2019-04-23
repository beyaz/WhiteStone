using BOA.EntityGeneration.Transforms;
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