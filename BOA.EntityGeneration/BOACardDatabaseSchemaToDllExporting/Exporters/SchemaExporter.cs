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
            context.Add(Data.SharedRepositoryClassOutput, new PaddedStringBuilder());
            context.Add(Data.TypeClassesOutput, new PaddedStringBuilder());
            context.Add(Data.BoaRepositoryClassesOutput, new PaddedStringBuilder());

            context.FireEvent(DataEvent.StartToExportSchema);

            context.Remove(Data.SharedRepositoryClassOutput);
            context.Remove(Data.TypeClassesOutput);
            context.Remove(Data.BoaRepositoryClassesOutput);
            context.Remove(Data.SchemaName);
        }
        #endregion
    }
}