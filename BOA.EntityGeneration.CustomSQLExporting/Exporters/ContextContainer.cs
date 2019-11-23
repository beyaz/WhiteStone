using System.Collections.Generic;
using BOA.DatabaseAccess;
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


        protected IDatabase database
        {
            get { return Data.Database[Context];}
            set
            {
                Context.Add(Data.Database, value);
            }
        }

        protected ConfigurationContract config
        {
            get { return Data.Config[Context];}
            set
            {
                Context.Add(Data.Config, value);
            }
        }


        protected CustomSqlNamingPatternContract customSqlNamingPattern
        {
            get => Data.CustomSqlNamingPattern[Context];
            set => Data.CustomSqlNamingPattern[Context] = value;
        }





        protected List<string>                   entityAssemblyReferences        => Data.EntityAssemblyReferences[Context];
        protected MsBuildQueue                   MsBuildQueue                    => Context.Get(MsBuildQueue.MsBuildQueueId);
        protected ProcessContract                processInfo                     => Data.ProcessInfo[Context];

        protected ProfileNamingPatternContract profileNamingPattern
        {
            get => Data.ProfileNamingPattern[Context];
            set => Data.ProfileNamingPattern[Context] = value;
        }

        protected List<string>                   repositoryAssemblyReference     => Data.RepositoryAssemblyReferences[Context];
        protected List<string>                   repositoryAssemblyReferenceList => Data.RepositoryAssemblyReferences[Context];
        protected List<string>                   repositoryAssemblyReferences    => Data.RepositoryAssemblyReferences[Context];
        #endregion
    }
}