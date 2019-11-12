using ___Company___.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace ___Company___.EntityGeneration.DataFlow
{
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
        public static readonly IDataConstant<Config> Config = new DataConstant<Config> {Id = nameof(Config)};

        /// <summary>
        ///     The custom SQL generation of profile identifier process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> CustomSqlGenerationOfProfileIdProcess = new DataConstant<ProcessInfo> {Id = nameof(CustomSqlGenerationOfProfileIdProcess)};

        /// <summary>
        ///     The schema generation process
        /// </summary>
        public static readonly IDataConstant<ProcessInfo> SchemaGenerationProcess = new DataConstant<ProcessInfo> {Id = nameof(SchemaGenerationProcess)};

        /// <summary>
        ///     The shared repository class output
        /// </summary>
        public static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryClassOutput = new DataConstant<PaddedStringBuilder> {Id = nameof(SharedRepositoryClassOutput)};

      
        #endregion
    }
}