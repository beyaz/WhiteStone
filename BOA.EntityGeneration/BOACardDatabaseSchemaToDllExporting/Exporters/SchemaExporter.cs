using BOA.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.SharedRepositoryFileExporting;

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
            context.Add(Data.BoaRepositoryFile, new PaddedStringBuilder());

            NamingPatternInitializer.Initialize(context);

            context.FireEvent(SchemaExportingEvent.SchemaExportStarted);
            context.FireEvent(SchemaExportingEvent.SchemaExportFinished);

            context.Remove(Data.BoaRepositoryFile);
            context.Remove(Data.SchemaName);
            NamingPatternInitializer.Remove(context);
        }
        #endregion
    }
}