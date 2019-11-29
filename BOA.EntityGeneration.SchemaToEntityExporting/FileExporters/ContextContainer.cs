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

        public IDatabase Database => Context.Database;
        #endregion

        #region Properties
        protected FileSystem                 FileSystem                                               => Context.FileSystem;
        protected MsBuildQueue               MsBuildQueue                                             => Context.MsBuildQueue;
        protected NamingPatternContract      NamingPattern                                            => Context.NamingPattern;
        protected ProcessContract            ProcessInfo                                              => Context.ProcessInfo;
        protected string                     SchemaName                                               => Context.SchemaName;
        protected string                     TableEntityClassNameForMethodParametersInRepositoryFiles => Context.TableEntityClassNameForMethodParametersInRepositoryFiles;
        protected ITableInfo                 TableInfo                                                => Context.TableInfo;
        protected TableNamingPatternContract TableNamingPattern                                       => Context.TableNamingPattern;
        #endregion

        #region Methods
        protected T Create<T>() where T : ContextContainer, new()
        {
            return new T {Context = Context};
        }
        #endregion
    }
}