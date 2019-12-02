using System.Collections.Generic;
using BOA.Collections;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    class ContextContainer
    {
        protected string Resolve(string value)
        {
            return Context.NamingMap.Resolve(value);
        }

        #region Public Properties
        public Context Context { get; set; }
        #endregion

        #region Properties
        protected CustomSQLExportingConfig          Config                       => Context.Config;
        protected CustomSqlInfo                  CustomSqlInfo                => Context.CustomSqlInfo;
        protected CustomSqlNamingPatternContract CustomSqlNamingPattern       => Context.CustomSqlNamingPattern;
        protected IDatabase                      Database                     => Context.Database;
        protected AddOnlyList<string>                   ExtraAssemblyReferencesForEntityProject     => Context.ExtraAssemblyReferencesForEntityProject;
        protected FileSystem                     FileSystem                   => Context.FileSystem;
        protected MsBuildQueue                   MsBuildQueue                 => Context.MsBuildQueue;
        protected ProcessContract                ProcessInfo                  => Context.ProcessInfo;
        protected string                         ProfileName                  => Context.ProfileName;
        protected ProfileNamingPatternContract   ProfileNamingPattern         => Context.ProfileNamingPattern;
        #endregion

        #region Methods
        protected T Create<T>() where T : ContextContainer, new()
        {
            return new T {Context = Context};
        }
        #endregion
    }
}