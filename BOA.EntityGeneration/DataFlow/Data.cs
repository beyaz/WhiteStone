using System.Collections.Generic;
using BOA.DatabaseAccess;
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
        internal static readonly IProperty<ConfigContract> Config = Property.Create<ConfigContract>();

        internal static readonly IProperty<IDatabase> Database = Property.Create<IDatabase>();

        internal static readonly IProperty<ITableInfo>   TableInfo          = Property.Create<ITableInfo>();
        internal static readonly IProperty<List<string>> TableNamesInSchema = Property.Create<List<string>>();
        #endregion

        #region Naming
        internal static readonly IProperty<string> SchemaName                                               = Property.Create<string>(nameof(SchemaName));
        internal static readonly IProperty<string> TableEntityClassNameForMethodParametersInRepositoryFiles = Property.Create<string>(nameof(TableEntityClassNameForMethodParametersInRepositoryFiles));
        #endregion

        #region Process Indicators
        internal static readonly IProperty<ProcessContract> ProcessInfo    = Property.Create<ProcessContract>(nameof(ProcessInfo));
        #endregion

        #region Files
        #endregion
    }
}