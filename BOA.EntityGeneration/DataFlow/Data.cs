using System.Collections.Generic;
using ___Company___.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.TfsAccess;

namespace ___Company___.EntityGeneration.DataFlow
{
    public static class DataEvent
    {
        public static readonly IEvent StartToExportSchema = new Event {Name = nameof(StartToExportSchema)};
        public static readonly IEvent AfterFetchedAllTableNamesInSchema = new Event {Name = nameof(AfterFetchedAllTableNamesInSchema)};
        public static readonly IEvent StartToExportTable = new Event {Name = nameof(StartToExportTable)};

        public static readonly IEvent StartToExportCustomSqlInfoProject = new Event {Name = nameof(StartToExportCustomSqlInfoProject)};


    }

    /// <summary>
    ///     The data
    /// </summary>
    public static class Data
    {
        public static readonly IDataConstant<ICustomSqlInfo>        CustomSqlInfo        = new DataConstant<ICustomSqlInfo> {Id        = nameof(CustomSqlInfo)};
        public static readonly IDataConstant<IProjectCustomSqlInfo> CustomSqlInfoProject = new DataConstant<IProjectCustomSqlInfo> {Id = nameof(CustomSqlInfoProject)};

        public static readonly IDataConstant<List<string>> TableNamesInSchema = new DataConstant<List<string>> {Id = nameof(TableNamesInSchema)};

        #region Static Fields
        /// <summary>
        ///     All schema generation process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> AllSchemaGenerationProcess = new DataConstant<ProcessInfo> {Id = nameof(AllSchemaGenerationProcess)};

        /// <summary>
        ///     The business class writer context
        /// </summary>
        public static readonly IDataConstant<BusinessClassWriterContext> BusinessClassWriterContext = new DataConstant<BusinessClassWriterContext> {Id = nameof(BusinessClassWriterContext)};

        /// <summary>
        ///     The configuration
        /// </summary>
        public static readonly IDataConstant<Config> Config = new DataConstant<Config> {Id = nameof(BOA.EntityGeneration.Config)};
        public static readonly IDataConstant<IDatabase> Database = new DataConstant<IDatabase> {Id = nameof(Database)};
        public static readonly IDataConstant<MsBuildQueue> MsBuildQueue = new DataConstant<MsBuildQueue> {Id = nameof(MsBuildQueue)};
        
        

        /// <summary>
        ///     The custom SQL generation of profile identifier process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> CustomSqlGenerationOfProfileIdProcess = new DataConstant<ProcessInfo> {Id = nameof(CustomSqlGenerationOfProfileIdProcess)};

        /// <summary>
        ///     The schema generation process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> SchemaGenerationProcess = new DataConstant<ProcessInfo> {Id = nameof(SchemaGenerationProcess)};

        public static readonly IDataConstant<FileSystem> FileAccess = new DataConstant<FileSystem> {Id = nameof(FileAccess)};

        /// <summary>
        ///     The shared repository class output
        /// </summary>
        public static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryClassOutput = new DataConstant<PaddedStringBuilder> {Id = nameof(SharedRepositoryClassOutput)};

        public static readonly IDataConstant<PaddedStringBuilder> TypeClassesOutput = new DataConstant<PaddedStringBuilder> {Id = nameof(TypeClassesOutput)};
        public static readonly IDataConstant<PaddedStringBuilder> BoaRepositoryClassesOutput = new DataConstant<PaddedStringBuilder> {Id = nameof(BoaRepositoryClassesOutput)};


        /// <summary>
        ///     The schema name
        /// </summary>
        public static readonly IDataConstant<string> SchemaName = new DataConstant<string> {Id = nameof(SchemaName)};

        /// <summary>
        ///     The table information
        /// </summary>
        public static readonly IDataConstant<BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces.ITableInfo> TableInfo = new DataConstant<BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces.ITableInfo> {Id = nameof(ITableInfo)};

      
        
        #endregion
    }
}