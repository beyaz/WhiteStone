using System.Collections.Generic;
using BOA.DataFlow;
using System;
using System.Collections.Generic;
using System.Threading;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    static class Data
    {
        public static readonly IDataConstant<string> ProfileName = DataConstant.Create<string>(nameof(ProfileName));

        public static readonly IDataConstant<CustomSqlInfo> CustomSqlInfo = DataConstant.Create<CustomSqlInfo>();

        public static readonly IDataConstant<ConfigurationContract> Config = DataConstant.Create<ConfigurationContract>(nameof(Config));

        public static readonly IDataConstant<List<string>> CustomSqlNamesInfProfile = DataConstant.Create<List<string>>(nameof(CustomSqlNamesInfProfile));

        public static readonly IDataConstant<IDatabase> Database = DataConstant.Create<IDatabase>();
        public static readonly IDataConstant<ProcessContract> ProcessInfo = DataConstant.Create<ProcessContract>(nameof(ProcessInfo));

        public static readonly IDataConstant<List<string>> RepositoryAssemblyReferences = DataConstant.Create<List<string>>(nameof(RepositoryAssemblyReferences));
        public static readonly IDataConstant<List<string>> EntityAssemblyReferences = DataConstant.Create<List<string>>(nameof(EntityAssemblyReferences));
        public static readonly IDataConstant<ProfileNamingPatternContract> ProfileNamingPattern = DataConstant.Create<ProfileNamingPatternContract>(nameof(ProfileNamingPatternContract));
        internal static readonly IDataConstant<CustomSqlNamingPatternContract> CustomSqlNamingPattern = DataConstant.Create<CustomSqlNamingPatternContract>(nameof(CustomSqlNamingPattern));
    }
}