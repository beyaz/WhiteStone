using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class ContextContainer
    {
        #region Public Properties
        public Context Context { get; set; }
        #endregion

        #region Properties
        protected ConfigurationContract          config                       => Context.config;
        protected CustomSqlInfo                  customSqlInfo                => Context.customSqlInfo;
        protected CustomSqlNamingPatternContract customSqlNamingPattern       => Context.customSqlNamingPattern;
        protected IDatabase                      database                     => Context.database;
        protected List<string>                   entityAssemblyReferences     => profileNamingPattern.EntityAssemblyReferences;
        protected FileSystem                     FileSystem                   => Context.FileSystem;
        protected MsBuildQueue                   MsBuildQueue                 => Context.MsBuildQueue;
        protected ProcessContract                processInfo                  => Context.processInfo;
        protected string                         ProfileName                  => Context.profileName;
        protected ProfileNamingPatternContract   profileNamingPattern         => Context.profileNamingPattern;
        protected List<string>                   repositoryAssemblyReferences => profileNamingPattern.RepositoryAssemblyReferences;
        #endregion

        #region Methods
        protected T Create<T>() where T : ContextContainer, new()
        {
            return new T {Context = Context};
        }
        #endregion
    }
}