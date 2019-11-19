using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    /// <summary>
    ///     The data
    /// </summary>
    static class Data
    {
        #region Static Fields
        /// <summary>
        ///     The configuration
        /// </summary>
        public static readonly IDataConstant<ConfigurationContract> Config = DataConstant.Create<ConfigurationContract>(nameof(Config));

        /// <summary>
        ///     The custom SQL information
        /// </summary>
        public static readonly IDataConstant<CustomSqlInfo> CustomSqlInfo = DataConstant.Create<CustomSqlInfo>();

        /// <summary>
        ///     The custom SQL names inf profile
        /// </summary>
        public static readonly IDataConstant<List<string>> CustomSqlNamesInfProfile = DataConstant.Create<List<string>>(nameof(CustomSqlNamesInfProfile));

        /// <summary>
        ///     The database
        /// </summary>
        public static readonly IDataConstant<IDatabase> Database = DataConstant.Create<IDatabase>();

        /// <summary>
        ///     The entity assembly references
        /// </summary>
        public static readonly IDataConstant<List<string>> EntityAssemblyReferences = DataConstant.Create<List<string>>(nameof(EntityAssemblyReferences));

        /// <summary>
        ///     The process information
        /// </summary>
        public static readonly IDataConstant<ProcessContract> ProcessInfo = DataConstant.Create<ProcessContract>(nameof(ProcessInfo));

        /// <summary>
        ///     The profile name
        /// </summary>
        public static readonly IDataConstant<string> ProfileName = DataConstant.Create<string>(nameof(ProfileName));

        /// <summary>
        ///     The profile naming pattern
        /// </summary>
        public static readonly IDataConstant<ProfileNamingPatternContract> ProfileNamingPattern = DataConstant.Create<ProfileNamingPatternContract>(nameof(ProfileNamingPatternContract));

        /// <summary>
        ///     The repository assembly references
        /// </summary>
        public static readonly IDataConstant<List<string>> RepositoryAssemblyReferences = DataConstant.Create<List<string>>(nameof(RepositoryAssemblyReferences));

        /// <summary>
        ///     The custom SQL naming pattern
        /// </summary>
        internal static readonly IDataConstant<CustomSqlNamingPatternContract> CustomSqlNamingPattern = DataConstant.Create<CustomSqlNamingPatternContract>(nameof(CustomSqlNamingPattern));
        #endregion
    }
}