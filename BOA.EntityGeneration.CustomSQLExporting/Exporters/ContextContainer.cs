using System.Collections.Generic;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class ContextContainer : BOA.DataFlow.ContextContainer
    {
        #region Properties
        protected CustomSqlInfo customSqlInfo
        {
            get { return Data.CustomSqlInfo[Context]; }
            set { Data.CustomSqlInfo[Context] = value; }
        }


        protected string profileName
        {
            get { return Data.ProfileName[Context];}
            set
            {
                Context.Add(Data.ProfileName, value);
            }
        }

        protected CustomSqlNamingPatternContract customSqlNamingPattern          => Data.CustomSqlNamingPattern[Context];
        protected List<string>                   entityAssemblyReferences        => Data.EntityAssemblyReferences[Context];
        protected MsBuildQueue                   MsBuildQueue                    => Context.Get(MsBuildQueue.MsBuildQueueId);
        protected ProcessContract                processInfo                     => Data.ProcessInfo[Context];
        protected ProfileNamingPatternContract   profileNamingPattern            => Data.ProfileNamingPattern[Context];
        protected List<string>                   repositoryAssemblyReference     => Data.RepositoryAssemblyReferences[Context];
        protected List<string>                   repositoryAssemblyReferenceList => Data.RepositoryAssemblyReferences[Context];
        protected List<string>                   repositoryAssemblyReferences    => Data.RepositoryAssemblyReferences[Context];
        #endregion
    }
}