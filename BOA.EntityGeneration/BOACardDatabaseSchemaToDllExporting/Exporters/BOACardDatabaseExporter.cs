using ___Company___.DataFlow;
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
        public static void Export(IDataContext context, string schemaName)
        {
            var schemaExporter = new SchemaExporter();

            schemaExporter.Export(context,schemaName);

            context.Get(Data.MsBuildQueue).Build();
        }

        public static void Export(IDataContext context)
        {
            var progress = context.Get(Data.AllSchemaGenerationProcess);
            var config = context.Get(Data.Config);

            var schemaNames = config.SchemaNamesToBeExport;

            progress.Total   = schemaNames.Count;
            progress.Current = 0;

            foreach (var schemaName in schemaNames)
            {
                progress.Text = $"Schema: {schemaName}";

                progress.Current++;

                var schemaExporter = new SchemaExporter();

                schemaExporter.Export(context,schemaName);
            }

            context.Get(Data.MsBuildQueue).Build();
            
        }
        #endregion
    }
}