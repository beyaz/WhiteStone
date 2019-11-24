﻿using System;
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

        #region Events

        #region Schema Exporting Events
        protected event Action OnSchemaExportStarted
        {
            add{ AttachEvent(SchemaExportingEvent.SchemaExportStarted, value);}
            remove => throw new NotImplementedException();
        }
        protected event Action OnSchemaExportFinished
        {
            add{ AttachEvent(SchemaExportingEvent.SchemaExportFinished, value);}
            remove => throw new NotImplementedException();
        }
        #endregion

        #region Table Exporting Events
        protected event Action OnTableExportFinished
        {
            add{ AttachEvent(TableExportingEvent.TableExportFinished, value);}
            remove => throw new NotImplementedException();
        }

        protected event Action OnTableExportStarted
        {
            add{ AttachEvent(TableExportingEvent.TableExportStarted, value);}
            remove => throw new NotImplementedException();
        }
        #endregion 
        #endregion

        #region Properties

        protected ConfigContract config
        {
            get => Data.Config[Context];
            set => Data.Config[Context] = value;
        }
        protected IDatabase database
        {
            get => Data.Database[Context];
            set => Data.Database[Context] = value;
        }

        protected MsBuildQueue MsBuildQueue
        {
            get => Context.Get(MsBuildQueue.MsBuildQueueId);
            set => Context.Add(MsBuildQueue.MsBuildQueueId, value);
        }
        protected ProcessContract processInfo
        {
            get=> Data.ProcessInfo[Context];
            set => Data.ProcessInfo[Context] = value;
        }

        protected NamingPatternContract namingPattern
        {
            get => NamingPatternContract.NamingPattern[Context];
            set => NamingPatternContract.NamingPattern[Context]=value;
        }
        
        protected ITableInfo                 tableInfo                                                => Data.TableInfo[Context];

        protected string tableEntityClassNameForMethodParametersInRepositoryFiles
        {
            get => Data.TableEntityClassNameForMethodParametersInRepositoryFiles[Context];
            set => Data.TableEntityClassNameForMethodParametersInRepositoryFiles[Context]=value;
        }

        protected string schemaName
        {
            get=> Data.SchemaName[Context];
            set=> Data.SchemaName[Context]=value;
        }

        protected TableNamingPatternContract tableNamingPattern
        {
            get => TableNamingPatternContract.TableNamingPattern[Context];
            set => TableNamingPatternContract.TableNamingPattern[Context]=value;
        }
        #endregion
    }
}