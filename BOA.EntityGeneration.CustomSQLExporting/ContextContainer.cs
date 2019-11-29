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
        protected CustomSQLExportingConfig          Config                       => Context.Config;
        protected CustomSqlInfo                  CustomSqlInfo                => Context.CustomSqlInfo;
        protected CustomSqlNamingPatternContract CustomSqlNamingPattern       => Context.CustomSqlNamingPattern;
        protected IDatabase                      Database                     => Context.Database;
        protected List<string>                   EntityAssemblyReferences     => ProfileNamingPattern.EntityAssemblyReferences;
        protected FileSystem                     FileSystem                   => Context.FileSystem;
        protected MsBuildQueue                   MsBuildQueue                 => Context.MsBuildQueue;
        protected ProcessContract                ProcessInfo                  => Context.ProcessInfo;
        protected string                         ProfileName                  => Context.ProfileName;
        protected ProfileNamingPatternContract   ProfileNamingPattern         => Context.ProfileNamingPattern;
        protected List<string>                   RepositoryAssemblyReferences => ProfileNamingPattern.RepositoryAssemblyReferences;
        #endregion

        #region Methods
        protected T Create<T>() where T : ContextContainer, new()
        {
            return new T {Context = Context};
        }
        #endregion
    }
}