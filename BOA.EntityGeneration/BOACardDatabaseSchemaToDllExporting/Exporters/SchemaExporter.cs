using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    /// <summary>
    ///     The schema exporter
    /// </summary>
    public static class SchemaExporter
    {
        #region Public Methods
        /// <summary>
        ///     Exports the specified schema name.
        /// </summary>
        public static void Export(IDataContext context, string schemaName)
        {
            context.Add(Data.SchemaName, schemaName);
            context.Add(Data.SharedRepositoryFile, new PaddedStringBuilder());
            context.Add(Data.EntityFile, new PaddedStringBuilder());
            context.Add(Data.BoaRepositoryFile, new PaddedStringBuilder());

            context.FireEvent(DataEvent.StartToExportSchema);

            context.Remove(Data.SharedRepositoryFile);
            context.Remove(Data.EntityFile);
            context.Remove(Data.BoaRepositoryFile);
            context.Remove(Data.SchemaName);
        }
        #endregion
    }
}