using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
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
            var Tracer = kernel.Get<Tracer>();

            var schemaNames = kernel.Get<BOACardDatabaseSchemaNames>().SchemaNames;

            Tracer.GenerateAllSchemaProcess.Total   = schemaNames.Length;
            Tracer.GenerateAllSchemaProcess.Current = 0;


            foreach (var schemaName in schemaNames)
            {
                Tracer.GenerateAllSchemaProcess.Text = $"Schema: {schemaName}";

                Tracer.GenerateAllSchemaProcess.Current++;

                var schemaExporter = kernel.Get<SchemaExporter>();

                schemaExporter.Export(schemaName);
            }

            kernel.Get<BatExporter>().ExportAllInOne();
        }
        #endregion
    }
}