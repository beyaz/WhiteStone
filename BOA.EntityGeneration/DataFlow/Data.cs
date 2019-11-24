
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.Models.Interfaces;

namespace BOA.EntityGeneration.DataFlow
{
    /// <summary>
    ///     The data
    /// </summary>
    static class Data
    {
        #region Static Fields

        

        internal static readonly Property<ITableInfo>   TableInfo          = Property.Create<ITableInfo>();
        #endregion

        #region Naming
        internal static readonly Property<string> SchemaName                                               = Property.Create<string>(nameof(SchemaName));
        internal static readonly Property<string> TableEntityClassNameForMethodParametersInRepositoryFiles = Property.Create<string>(nameof(TableEntityClassNameForMethodParametersInRepositoryFiles));
        #endregion

        #region Process Indicators
        internal static readonly Property<ProcessContract> ProcessInfo    = Property.Create<ProcessContract>(nameof(ProcessInfo));
        #endregion

        #region Files
        #endregion
    }
}