using BOA.DataFlow;
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
            context.OpenBracket();

            context.Add(Data.SchemaName, schemaName);

            NamingPatternInitializer.Initialize(context);

            context.FireEvent(SchemaExportingEvent.SchemaExportStarted);
            context.FireEvent(SchemaExportingEvent.SchemaExportFinished);

            context.CloseBracket();
        }
        #endregion
    }
}