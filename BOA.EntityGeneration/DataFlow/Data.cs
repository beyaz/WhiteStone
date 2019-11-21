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
        internal static readonly IDataConstant<ConfigContract> Config = DataConstant.Create<ConfigContract>();

        internal static readonly IDataConstant<IDatabase> Database = DataConstant.Create<IDatabase>();

        internal static readonly IDataConstant<ITableInfo>   TableInfo          = DataConstant.Create<ITableInfo>();
        internal static readonly IDataConstant<List<string>> TableNamesInSchema = DataConstant.Create<List<string>>();
        #endregion

        #region Naming
        internal static readonly IDataConstant<string> SchemaName                                               = DataConstant.Create<string>(nameof(SchemaName));
        internal static readonly IDataConstant<string> TableEntityClassNameForMethodParametersInRepositoryFiles = DataConstant.Create<string>(nameof(TableEntityClassNameForMethodParametersInRepositoryFiles));
        #endregion

        #region Process Indicators
        internal static readonly IDataConstant<ProcessContract> SchemaGenerationProcess    = DataConstant.Create<ProcessContract>(nameof(SchemaGenerationProcess));
        #endregion

        #region Files
        #endregion
    }
}