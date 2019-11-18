using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.Exporters
{
    public class BOACardDatabaseExporter
    {
        #region Public Methods
        public static void Export(IDataContext context)
        {
            var progress = context.Get(Data.AllSchemaGenerationProcess);
            var config   = context.Get(Data.Config);

            var schemaNames = config.SchemaNamesToBeExport.Where(name => name != "*").ToList();

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