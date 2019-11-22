using System.Collections.Generic;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
     class ContextContainer : BOA.DataFlow.ContextContainer
    {
        #region Properties
        protected CustomSqlInfo                  customSqlInfo            => Data.CustomSqlInfo[Context];
        protected CustomSqlNamingPatternContract customSqlNamingPattern   => Data.CustomSqlNamingPattern[Context];
        protected List<string>                   entityAssemblyReferences => Data.EntityAssemblyReferences[Context];
        protected ProcessContract                processInfo              => Data.ProcessInfo[Context];
        protected ProfileNamingPatternContract   profileNamingPattern     => Data.ProfileNamingPattern[Context];

        protected List<string> repositoryAssemblyReferenceList => Data.RepositoryAssemblyReferences[Context];
        #endregion
    }
}