using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class BOACardDatabaseExporter
    {
        

        #region Public Methods
        public static void Export(Kernel kernel, string schemaName)
        {
            var schemaExporter = kernel.Get<SchemaExporter>();

            schemaExporter.Export(schemaName);
        }

        public static void Export(Kernel kernel)
        {
            var schemaNames = kernel.Get<BOACardDatabaseSchemaNames>().SchemaNames;
            foreach (var schemaName in schemaNames)
            {
                var schemaExporter = kernel.Get<SchemaExporter>();

                schemaExporter.Export(schemaName);
            }

            kernel.Get<BatExporter>().ExportAllInOne();
        }
        #endregion
    }
}