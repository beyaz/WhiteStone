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
        public static readonly IProperty<ConfigurationContract> Config = Property.Create<ConfigurationContract>(nameof(Config));

        /// <summary>
        ///     The custom SQL information
        /// </summary>
        public static readonly IProperty<CustomSqlInfo> CustomSqlInfo = Property.Create<CustomSqlInfo>();

        /// <summary>
        ///     The custom SQL names inf profile
        /// </summary>
        public static readonly IProperty<IReadOnlyList<string>> CustomSqlNamesInfProfile = Property.Create<IReadOnlyList<string>>(nameof(CustomSqlNamesInfProfile));

        /// <summary>
        ///     The database
        /// </summary>
        public static readonly IProperty<IDatabase> Database = Property.Create<IDatabase>();

        /// <summary>
        ///     The entity assembly references
        /// </summary>
        public static readonly IProperty<List<string>> EntityAssemblyReferences = Property.Create<List<string>>(nameof(EntityAssemblyReferences));

        /// <summary>
        ///     The process information
        /// </summary>
        public static readonly IProperty<ProcessContract> ProcessInfo = Property.Create<ProcessContract>(nameof(ProcessInfo));

        /// <summary>
        ///     The profile name
        /// </summary>
        public static readonly IProperty<string> ProfileName = Property.Create<string>(nameof(ProfileName));

        /// <summary>
        ///     The profile naming pattern
        /// </summary>
        public static readonly IProperty<ProfileNamingPatternContract> ProfileNamingPattern = Property.Create<ProfileNamingPatternContract>(nameof(ProfileNamingPatternContract));

        /// <summary>
        ///     The repository assembly references
        /// </summary>
        public static readonly IProperty<List<string>> RepositoryAssemblyReferences = Property.Create<List<string>>(nameof(RepositoryAssemblyReferences));

        /// <summary>
        ///     The custom SQL naming pattern
        /// </summary>
        internal static readonly IProperty<CustomSqlNamingPatternContract> CustomSqlNamingPattern = Property.Create<CustomSqlNamingPatternContract>(nameof(CustomSqlNamingPattern));
        #endregion
    }
}