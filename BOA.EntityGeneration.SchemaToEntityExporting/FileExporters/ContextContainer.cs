using System;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.SchemaToEntityExporting.DbModels;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class ContextContainer
    {
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
        public Context Context { get; protected set; }

        public IDatabase Database => Context.database;
        #endregion

        #region Properties
        protected ConfigContract             Config                                                   => Context.config;
        protected FileSystem                 FileSystem                                               => Context.FileSystem;
        protected MsBuildQueue               MsBuildQueue                                             => Context.MsBuildQueue;
        protected NamingPatternContract      NamingPattern                                            => Context.namingPattern;
        protected ProcessContract            ProcessInfo                                              => Context.processInfo;
        protected string                     SchemaName                                               => Context.SchemaName;
        protected string                     TableEntityClassNameForMethodParametersInRepositoryFiles => Context.tableEntityClassNameForMethodParametersInRepositoryFiles;
        protected ITableInfo                 TableInfo                                                => Context.tableInfo;
        protected TableNamingPatternContract TableNamingPattern                                       => Context.tableNamingPattern;
        #endregion

        #region Methods
        protected T Create<T>() where T : ContextContainer, new()
        {
            return new T {Context = Context};
        }
        #endregion
    }
}