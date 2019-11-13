using System.Collections.Generic;
using ___Company___.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace ___Company___.EntityGeneration.DataFlow
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
        #region Static Fields
        /// <summary>
        ///     All schema generation process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> AllSchemaGenerationProcess = DataConstant.Create<ProcessInfo>();

        public static readonly IDataConstant<PaddedStringBuilder> BoaRepositoryClassesOutput = DataConstant.Create<PaddedStringBuilder>();

        /// <summary>
        ///     The business class writer context
        /// </summary>
        public static readonly IDataConstant<BusinessClassWriterContext> BusinessClassWriterContext = DataConstant.Create<BusinessClassWriterContext>();

        /// <summary>
        ///     The configuration
        /// </summary>
        public static readonly IDataConstant<Config> Config = DataConstant.Create<Config>();

        /// <summary>
        ///     The custom SQL generation of profile identifier process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> CustomSqlGenerationOfProfileIdProcess = DataConstant.Create<ProcessInfo>();

        public static readonly IDataConstant<ICustomSqlInfo>        CustomSqlInfo        = DataConstant.Create<ICustomSqlInfo>();
        public static readonly IDataConstant<IProjectCustomSqlInfo> CustomSqlInfoProject = DataConstant.Create<IProjectCustomSqlInfo>();
        public static readonly IDataConstant<IDatabase>             Database             = DataConstant.Create<IDatabase>();

        public static readonly IDataConstant<FileSystem>   FileAccess   = DataConstant.Create<FileSystem>();
        public static readonly IDataConstant<MsBuildQueue> MsBuildQueue = DataConstant.Create<MsBuildQueue>();

        /// <summary>
        ///     The schema generation process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> SchemaGenerationProcess = DataConstant.Create<ProcessInfo>();

        /// <summary>
        ///     The schema name
        /// </summary>
        public static readonly IDataConstant<string> SchemaName = DataConstant.Create<string>();

        /// <summary>
        ///     The shared repository class output
        /// </summary>
        public static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryClassOutput = DataConstant.Create<PaddedStringBuilder>();

        /// <summary>
        ///     The table information
        /// </summary>
        public static readonly IDataConstant<ITableInfo> TableInfo = DataConstant.Create<ITableInfo>();

        public static readonly IDataConstant<List<string>> TableNamesInSchema = DataConstant.Create<List<string>>();

        public static readonly IDataConstant<PaddedStringBuilder> TypeClassesOutput = DataConstant.Create<PaddedStringBuilder>();
        #endregion
    }
}