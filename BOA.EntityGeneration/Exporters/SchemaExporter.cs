using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BoaRepositoryFileExporting;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;

namespace BOA.EntityGeneration.Exporters
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
            context.Add(BoaRepositoryFileExporter.File, new PaddedStringBuilder());

            NamingPatternInitializer.Initialize(context);

            context.FireEvent(SchemaExportingEvent.SchemaExportStarted);
            context.FireEvent(SchemaExportingEvent.SchemaExportFinished);

            context.Remove(BoaRepositoryFileExporter.File);
            context.Remove(Data.SchemaName);
            NamingPatternInitializer.Remove(context);
        }
        #endregion
    }
}