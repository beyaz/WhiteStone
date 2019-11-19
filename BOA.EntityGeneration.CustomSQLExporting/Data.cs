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
        public static readonly IDataConstant<List<string>> RepositoryAssemblyReferences = DataConstant.Create<List<string>>(nameof(RepositoryAssemblyReferences));
        public static readonly IDataConstant<List<string>> EntityAssemblyReferences = DataConstant.Create<List<string>>(nameof(EntityAssemblyReferences));
        public static readonly IDataConstant<ProfileNamingPatternContract> ProfileNamingPattern = DataConstant.Create<ProfileNamingPatternContract>(nameof(ProfileNamingPatternContract));
        internal static readonly IDataConstant<CustomSqlNamingPatternContract> CustomSqlNamingPattern = DataConstant.Create<CustomSqlNamingPatternContract>(nameof(CustomSqlNamingPattern));
    }
}