using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class BOACardDatabaseExporter
    {
        #region Static Fields
        public static readonly string[] SchemaNames =
        {
            "BKM",
            "BNS",
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
            "POS",
            "PPD",
            "PRM",
            "RKL",
            "STM",
            "SWC",
            "TMS",
            "TRN",
            "VIS"
        };
        #endregion

        #region Public Methods
        public static void Export()
        {
            using (var kernel = new Kernel())
            {
                Export(kernel);

// TODO: ???                kernel.Get<BOACardDatabase>().Dispose();
            }
        }

        public static void Export(Kernel kernel)
        {
            foreach (var schemaName in SchemaNames)
            {
                var schemaExporter = kernel.Get<SchemaExporter>();

                schemaExporter.Export(schemaName);
            }
        }
        #endregion
    }
}