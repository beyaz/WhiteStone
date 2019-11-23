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
        public static readonly Property<ConfigurationContract> Config = Property.Create<ConfigurationContract>(nameof(Config));

        /// <summary>
        ///     The custom SQL information
        /// </summary>
        public static readonly Property<CustomSqlInfo> CustomSqlInfo = Property.Create<CustomSqlInfo>();

        

        /// <summary>
        ///     The database
        /// </summary>
        public static readonly Property<IDatabase> Database = Property.Create<IDatabase>();

        /// <summary>
        ///     The entity assembly references
        /// </summary>
        public static readonly Property<List<string>> EntityAssemblyReferences = Property.Create<List<string>>(nameof(EntityAssemblyReferences));

        /// <summary>
        ///     The process information
        /// </summary>
        public static readonly Property<ProcessContract> ProcessInfo = Property.Create<ProcessContract>(nameof(ProcessInfo));

        /// <summary>
        ///     The profile name
        /// </summary>
        public static readonly Property<string> ProfileName = Property.Create<string>(nameof(ProfileName));

        /// <summary>
        ///     The profile naming pattern
        /// </summary>
        public static readonly Property<ProfileNamingPatternContract> ProfileNamingPattern = Property.Create<ProfileNamingPatternContract>(nameof(ProfileNamingPatternContract));

        /// <summary>
        ///     The repository assembly references
        /// </summary>
        public static readonly Property<List<string>> RepositoryAssemblyReferences = Property.Create<List<string>>(nameof(RepositoryAssemblyReferences));

        /// <summary>
        ///     The custom SQL naming pattern
        /// </summary>
        internal static readonly Property<CustomSqlNamingPatternContract> CustomSqlNamingPattern = Property.Create<CustomSqlNamingPatternContract>(nameof(CustomSqlNamingPattern));
        #endregion
    }
}