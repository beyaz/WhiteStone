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
            for (var i = 0; i < schemaNames.Length; i++)
            {
                var schemaName = schemaNames[i];

                Tracer.GenerateAllSchemaProcess.Text = $"Schema: {schemaName}";

                Tracer.GenerateAllSchemaProcess.PercentageOfCompletion = (i / schemaNames.Length) * 100;

                var schemaExporter = kernel.Get<SchemaExporter>();

                schemaExporter.Export(schemaName);
            }

            kernel.Get<BatExporter>().ExportAllInOne();
        }
        #endregion
    }
}