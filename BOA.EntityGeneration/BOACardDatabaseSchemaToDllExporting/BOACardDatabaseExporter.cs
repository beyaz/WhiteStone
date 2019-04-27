using BOA.DatabaseAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class BOACardDatabaseExporter
    {
        #region Public Methods
        public static void Export()
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

                foreach (var schemaName in schemaNames)
                {
                    var schemaExporter = kernel.Get<SchemaExporter>();

                    schemaExporter.Export(schemaName);
                }

                kernel.Get<BOACardDatabase>().Dispose();
            }
        }
        #endregion
    }
}