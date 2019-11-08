using System.Collections.Generic;
using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    /// <summary>
    ///     The schema exporter data preparer
    /// </summary>
    public class SchemaExporterDataPreparer
    {
        #region Static Fields
        /// <summary>
        ///     The cache
        /// </summary>
        static readonly Dictionary<string, IReadOnlyList<ITableInfo>> Cache = new Dictionary<string, IReadOnlyList<ITableInfo>>();
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        [Inject]
        public Config Config { get; set; }

        /// <summary>
        ///     Gets or sets the database.
        /// </summary>
        [Inject]
        public IDatabase Database { get; set; }

        /// <summary>
        ///     Gets or sets the generator data creator.
        /// </summary>
        [Inject]
        public GeneratorDataCreator GeneratorDataCreator { get; set; }

        /// <summary>
        ///     Gets or sets the table information DAO.
        /// </summary>
        [Inject]
        public TableInfoDao TableInfoDao { get; set; }

        /// <summary>
        ///     Gets or sets the tracer.
        /// </summary>
        [Inject]
        public Tracer Tracer { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the table information.
        /// </summary>
        public ITableInfo GetTableInfo(string schemaName, string tableName)
        {
            var tableInfo = TableInfoDao.GetInfo(Config.TableCatalog, schemaName, tableName);

            var generatorData = GeneratorDataCreator.Create(tableInfo);
            return generatorData;
        }

        /// <summary>
        ///     Prepares the specified schema name.
        /// </summary>
        public IReadOnlyList<ITableInfo> Prepare(string schemaName)
        {
            if (Cache.ContainsKey(schemaName))
            {
                Tracer.Trace($"Fetching from cache {schemaName}");
                return Cache[schemaName];
            }

            var tableNames = SchemaInfo.GetAllTableNamesInSchema(Database, schemaName).ToList();

            tableNames = tableNames.Where(x => IsReadyToExport(schemaName, x)).ToList();

            var items = new List<ITableInfo>();

            Tracer.SchemaGenerationProcess.Total   = tableNames.Count;
            Tracer.SchemaGenerationProcess.Current = 0;

            foreach (var tableName in tableNames)
            {
                Tracer.SchemaGenerationProcess.Text = $"Fetching table information of {tableName}";
                Tracer.SchemaGenerationProcess.Current++;

                var generatorData = GetTableInfo(schemaName, tableName);

                items.Add(generatorData);
            }

            Cache[schemaName] = items;

            return items;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is ready to export] [the specified schema name].
        /// </summary>
        bool IsReadyToExport(string schemaName, string tableName)
        {
            var fullTableName = $"{schemaName}.{tableName}";

            var isNotExportable = Config.NotExportableTables.Contains(fullTableName);
            if (isNotExportable)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}