using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
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

            kernel.Get<MsBuildQueue>().Build();
        }

        public static void Export(Kernel kernel)
        {
            var Tracer = kernel.Get<Tracer>();

            var schemaNames = kernel.Get<BOACardDatabaseSchemaNames>().SchemaNames;

            Tracer.AllSchemaGenerationProcess.Total   = schemaNames.Length;
            Tracer.AllSchemaGenerationProcess.Current = 0;

            foreach (var schemaName in schemaNames)
            {
                Tracer.AllSchemaGenerationProcess.Text = $"Schema: {schemaName}";

                Tracer.AllSchemaGenerationProcess.Current++;

                var schemaExporter = kernel.Get<SchemaExporter>();

                schemaExporter.Export(schemaName);
            }

            kernel.Get<MsBuildQueue>().Build();
            
        }
        #endregion
    }
}