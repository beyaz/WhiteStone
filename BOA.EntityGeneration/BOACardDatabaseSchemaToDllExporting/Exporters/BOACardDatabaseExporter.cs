using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;

using static ___Company___.EntityGeneration.DataFlow.DataContext;
using ___Company___.EntityGeneration.DataFlow;


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
            var progress = Context.Get(Data.AllSchemaGenerationProcess);

            var schemaNames = kernel.Get<Config>().SchemaNamesToBeExport;

            progress.Total   = schemaNames.Count;
            progress.Current = 0;

            foreach (var schemaName in schemaNames)
            {
                progress.Text = $"Schema: {schemaName}";

                progress.Current++;

                var schemaExporter = kernel.Get<SchemaExporter>();

                schemaExporter.Export(schemaName);
            }

            kernel.Get<MsBuildQueue>().Build();
            
        }
        #endregion
    }
}