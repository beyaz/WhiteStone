using System.Collections.Generic;
using BOA.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.DataFlow
{
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
        #region Naming
        public static readonly IDataConstant<string> SchemaName = DataConstant.Create<string>();
        public static readonly IDataConstant<string> TableEntityClassNameForMethodParametersInRepositoryFiles = DataConstant.Create<string>();
        public static readonly IDataConstant<string> RepositoryClassName = DataConstant.Create<string>();
        public static readonly IDataConstant<string> BusinessClassNamespace = DataConstant.Create<string>();
        #endregion

        #region Static Fields
        public static readonly IDataConstant<Config>                     Config                     = DataConstant.Create<Config>();
        public static readonly IDataConstant<ICustomSqlInfo>             CustomSqlInfo              = DataConstant.Create<ICustomSqlInfo>();
        public static readonly IDataConstant<IProjectCustomSqlInfo>      CustomSqlInfoProject       = DataConstant.Create<IProjectCustomSqlInfo>();
        public static readonly IDataConstant<IDatabase>                  Database                   = DataConstant.Create<IDatabase>();
        public static readonly IDataConstant<FileSystem>                 FileAccess                 = DataConstant.Create<FileSystem>();
        public static readonly IDataConstant<MsBuildQueue>               MsBuildQueue               = DataConstant.Create<MsBuildQueue>();
        
        public static readonly IDataConstant<ITableInfo>                 TableInfo                  = DataConstant.Create<ITableInfo>();
        public static readonly IDataConstant<List<string>>               TableNamesInSchema         = DataConstant.Create<List<string>>();
        #endregion

        #region Process Indicators
        public static readonly IDataConstant<ProcessInfo> AllSchemaGenerationProcess            = DataConstant.Create<ProcessInfo>();
        public static readonly IDataConstant<ProcessInfo> CustomSqlGenerationOfProfileIdProcess = DataConstant.Create<ProcessInfo>();
        public static readonly IDataConstant<ProcessInfo> SchemaGenerationProcess               = DataConstant.Create<ProcessInfo>();
        #endregion

        #region Files
        public static readonly IDataConstant<PaddedStringBuilder> EntityFile           = DataConstant.Create<PaddedStringBuilder>();
        public static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryFile = DataConstant.Create<PaddedStringBuilder>();
        public static readonly IDataConstant<PaddedStringBuilder> BoaRepositoryFile    = DataConstant.Create<PaddedStringBuilder>();
        #endregion
    }
}