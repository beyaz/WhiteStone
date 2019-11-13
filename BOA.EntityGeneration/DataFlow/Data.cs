using ___Company___.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.TfsAccess;

namespace ___Company___.EntityGeneration.DataFlow
{
    public static class DataEvent
    {
        /// <summary>
        ///     The before business class export
        /// </summary>
        public static readonly IEvent BeforeBusinessClassExport = new Event {Name = nameof(BeforeBusinessClassExport)};
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

        public static readonly IDataConstant<FileAccess> FileAccess = new DataConstant<FileAccess> {Id = nameof(FileAccess)};

        /// <summary>
        ///     The shared repository class output
        /// </summary>
        public static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryClassOutput = new DataConstant<PaddedStringBuilder> {Id = nameof(SharedRepositoryClassOutput)};


        /// <summary>
        ///     The schema name
        /// </summary>
        public static readonly IDataConstant<string> SchemaName = new DataConstant<string> {Id = nameof(SchemaName)};

        /// <summary>
        ///     The table information
        /// </summary>
        public static readonly IDataConstant<ITableInfo> TableInfo = new DataConstant<ITableInfo> {Id = nameof(ITableInfo)};

      
        public static readonly IDataConstant<PaddedStringBuilder> TypesFileOutput = new DataConstant<PaddedStringBuilder> {Id = nameof(TypesFileOutput)};
        #endregion
    }
}