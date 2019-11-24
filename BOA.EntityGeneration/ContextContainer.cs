using System;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.Models.Interfaces;
using BOA.EntityGeneration.Naming;

namespace BOA.EntityGeneration
{
    class ContextContainer 
    {
        public Context Context { get; protected set; }
        protected T Create<T>() where T : ContextContainer, new()
        {
            return new T {Context = Context};
        }

        #region Events
        protected event Action SchemaExportFinished
        {
            add => Context.SchemaExportFinished += value;
            remove => throw new NotImplementedException();
        }

        protected event Action SchemaExportStarted
        {
            add => Context.SchemaExportStarted += value;
            remove => throw new NotImplementedException();
        }

        protected event Action TableExportFinished
        {
            add => Context.TableExportFinished += value;
            remove => throw new NotImplementedException();
        }

        protected event Action TableExportStarted
        {
            add => Context.TableExportStarted += value;
            remove => throw new NotImplementedException();
        }
        #endregion

        #region Public Properties
       
        public     IDatabase database => Context.database;
        #endregion

        #region Properties
        protected ConfigContract             config                                                   => Context.config;
        protected MsBuildQueue               MsBuildQueue                                             => Context.MsBuildQueue;
        protected NamingPatternContract      namingPattern                                            => Context.namingPattern;
        protected ProcessContract            processInfo                                              => Context.processInfo;
        protected string                     SchemaName                                               => Context.SchemaName;
        protected string                     tableEntityClassNameForMethodParametersInRepositoryFiles => Context.tableEntityClassNameForMethodParametersInRepositoryFiles;
        protected ITableInfo                 tableInfo                                                => Context.tableInfo;
        protected TableNamingPatternContract tableNamingPattern                                       => Context.tableNamingPattern;
        #endregion
    }
}