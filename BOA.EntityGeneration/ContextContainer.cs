using System;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Models.Interfaces;
using BOA.EntityGeneration.Naming;

namespace BOA.EntityGeneration
{
    class ContextContainer : BOA.DataFlow.ContextContainer
    {
        public new BOA.EntityGeneration.Context Context { get; set; }

        #region Events
        protected event Action SchemaExportFinished
        {
            add => Context.SchemaExportFinished+= value;
            remove => throw new NotImplementedException();
        }

        protected event Action SchemaExportStarted
        {
            add => Context.SchemaExportStarted+= value;
            remove => throw new NotImplementedException();
        }


        protected event Action TableExportFinished
        {
            add => Context.TableExportFinished+=value;
            remove => throw new NotImplementedException();
        }

        protected event Action TableExportStarted
        {
            add => Context.TableExportStarted += value;
            remove => throw new NotImplementedException();
        }
        #endregion

        #region Properties
        protected ConfigContract config {get;  set;}

        public IDatabase database{get;protected set;}

        protected MsBuildQueue MsBuildQueue => Context.MsBuildQueue;

        protected NamingPatternContract namingPattern {get; set;}

        protected ProcessContract processInfo
        {
            get => Data.ProcessInfo[Context];
            set => Data.ProcessInfo[Context] = value;
        }

        protected string schemaName {get; set;}

        protected string tableEntityClassNameForMethodParametersInRepositoryFiles
        {
            get => Data.TableEntityClassNameForMethodParametersInRepositoryFiles[Context];
            set => Data.TableEntityClassNameForMethodParametersInRepositoryFiles[Context] = value;
        }

        protected ITableInfo tableInfo
        {
            get => Data.TableInfo[Context];
            set => Data.TableInfo[Context] = value;
        }

        protected TableNamingPatternContract tableNamingPattern
        {
            get => TableNamingPatternContract.TableNamingPattern[Context];
            set => TableNamingPatternContract.TableNamingPattern[Context] = value;
        }
        #endregion
    }
}