using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class BOACardDatabaseExporter
    {
        #region Public Methods
        public static void Export(IDataContext context, string schemaName)
        {
            SchemaExporter.Export(context, schemaName);
        }

        public static void Export(IDataContext context)
        {
            var progress = context.Get(Data.AllSchemaGenerationProcess);
            var config   = context.Get(Data.Config);

            var schemaNames = config.SchemaNamesToBeExport;

            progress.Total   = schemaNames.Count;
            progress.Current = 0;

            foreach (var schemaName in schemaNames)
            {
                progress.Text = $"Schema: {schemaName}";

                progress.Current++;

                SchemaExporter.Export(context, schemaName);
            }

        }
        #endregion
    }
}