using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModelDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

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

            using (var kernel = new StandardKernel())
            {
                kernel.Bind<Database>().To<BOACardDatabase>().InSingletonScope();
                kernel.Bind<IDatabase>().To<BOACardDatabase>().InSingletonScope();

                using (var database = kernel.Get<BOACardDatabase>())
                {
                    foreach (var schemaName in schemaNames)
                    {
                        var schemaExporter = kernel.Get<SchemaExporter>();

                        schemaExporter.Export(new SchemaExporterDataForBOACard
                        {
                            Database   = database,
                            SchemaName = schemaName
                        });
                    }
                }
            }


            
        }
        #endregion
    }
}