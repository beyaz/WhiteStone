using System;
using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class ContextContainer : DataFlow.ContextContainer
    {
        

        #region Properties
        protected ConfigurationContract config
        {
            get { return Data.Config[Context]; }
            set { Context.Add(Data.Config, value); }
        }

        protected CustomSqlInfo customSqlInfo
        {
            get { return Data.CustomSqlInfo[Context]; }
            set { Data.CustomSqlInfo[Context] = value; }
        }

        protected CustomSqlNamingPatternContract customSqlNamingPattern
        {
            get => Data.CustomSqlNamingPattern[Context];
            set => Data.CustomSqlNamingPattern[Context] = value;
        }

        protected IDatabase database
        {
            get { return Data.Database[Context]; }
            set { Context.Add(Data.Database, value); }
        }

        protected List<string> entityAssemblyReferences => Data.EntityAssemblyReferences[Context];

        protected MsBuildQueue_old MsBuildQueue
        {
            get => Context.Get(MsBuildQueue_old.MsBuildQueueId);
            set => Context.Add(MsBuildQueue_old.MsBuildQueueId, value);
        }

        protected ProcessContract processInfo
        {
            get=> Data.ProcessInfo[Context];
            set=> Data.ProcessInfo[Context]=value;
        }
        protected ProcessContract MsBuildQueueProcess
        {
            set=> MsBuildQueue_old.ProcessInfo[Context]=value;
        }
        protected string profileName
        {
            get { return Data.ProfileName[Context]; }
            set { Context.Add(Data.ProfileName, value); }
        }

        protected ProfileNamingPatternContract profileNamingPattern
        {
            get => Data.ProfileNamingPattern[Context];
            set => Data.ProfileNamingPattern[Context] = value;
        }

        protected List<string> repositoryAssemblyReference     => Data.RepositoryAssemblyReferences[Context];
        protected List<string> repositoryAssemblyReferenceList => Data.RepositoryAssemblyReferences[Context];
        protected List<string> repositoryAssemblyReferences    => Data.RepositoryAssemblyReferences[Context];
        #endregion
    }
}