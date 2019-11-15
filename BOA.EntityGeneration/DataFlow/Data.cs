using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.DataFlow
{

    public static class CustomSqlExportingEvent
    {
        #region Static Fields
        public static readonly IEvent StartedToExportProfileId = new Event {Name = nameof(StartedToExportProfileId)};
        public static readonly IEvent StartedToExportObjectId= new Event {Name = nameof(StartedToExportObjectId)};
        #endregion
    }

    public static class CustomSqlExportingData
    {
        #region Static Fields
        
        #endregion
    }

    public static class DataEvent
    {
        #region Static Fields
        public static readonly IEvent AfterFetchedAllTableNamesInSchema = new Event {Name = nameof(AfterFetchedAllTableNamesInSchema)};
        public static readonly IEvent StartToExportCustomSqlInfoProject = new Event {Name = nameof(StartToExportCustomSqlInfoProject)};
        public static readonly IEvent StartToExportSchema               = new Event {Name = nameof(StartToExportSchema)};
        public static readonly IEvent StartToExportTable                = new Event {Name = nameof(StartToExportTable)};
        #endregion
    }

    /// <summary>
    ///     The data
    /// </summary>
    public static class Data
    {
        #region Static Fields
        public static readonly IDataConstant<ConfigContract>        Config               = DataConstant.Create<ConfigContract>();
        
        public static readonly IDataConstant<IDatabase>             Database             = DataConstant.Create<IDatabase>();
        public static readonly IDataConstant<FileSystem>            FileAccess           = DataConstant.Create<FileSystem>();
        public static readonly IDataConstant<MsBuildQueue>          MsBuildQueue         = DataConstant.Create<MsBuildQueue>();
        public static readonly IDataConstant<ITableInfo>            TableInfo            = DataConstant.Create<ITableInfo>();
        public static readonly IDataConstant<List<string>>          TableNamesInSchema   = DataConstant.Create<List<string>>();
        #endregion

        #region Naming
        public static readonly IDataConstant<string> SchemaName                                               = DataConstant.Create<string>(nameof(SchemaName));
        public static readonly IDataConstant<string> TableEntityClassNameForMethodParametersInRepositoryFiles = DataConstant.Create<string>(nameof(TableEntityClassNameForMethodParametersInRepositoryFiles));
        public static readonly IDataConstant<string> RepositoryClassName                                      = DataConstant.Create<string>(nameof(RepositoryClassName));
        public static readonly IDataConstant<string> SharedRepositoryClassName = DataConstant.Create<string>(nameof(SharedRepositoryClassName));
        public static readonly IDataConstant<string> BusinessClassNamespace                                   = DataConstant.Create<string>(nameof(BusinessClassNamespace));
        #endregion

        #region Process Indicators
        public static readonly IDataConstant<ProcessInfo> AllSchemaGenerationProcess            = DataConstant.Create<ProcessInfo>(nameof(AllSchemaGenerationProcess));
        public static readonly IDataConstant<ProcessInfo> CustomSqlGenerationOfProfileIdProcess = DataConstant.Create<ProcessInfo>(nameof(CustomSqlGenerationOfProfileIdProcess));
        public static readonly IDataConstant<ProcessInfo> SchemaGenerationProcess               = DataConstant.Create<ProcessInfo>(nameof(SchemaGenerationProcess));
        #endregion

        #region Files
        public static readonly IDataConstant<PaddedStringBuilder> EntityFile           = DataConstant.Create<PaddedStringBuilder>(nameof(EntityFile));
        public static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryFile = DataConstant.Create<PaddedStringBuilder>(nameof(SharedRepositoryFile));
        public static readonly IDataConstant<PaddedStringBuilder> BoaRepositoryFile    = DataConstant.Create<PaddedStringBuilder>(nameof(BoaRepositoryFile));
        #endregion
    }
}