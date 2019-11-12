using System.Collections.Generic;
using System.Linq;
using ___Company___.EntityGeneration.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

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

        #region Public Methods
        /// <summary>
        ///     Gets the table information.
        /// </summary>
        public ITableInfo GetTableInfo(string schemaName, string tableName)
        {
            var config   = Context.Get(Data.Config);
            var database = Context.Get(Data.Database);

            var tableInfoDao = new TableInfoDao {Database = database, IndexInfoAccess = new IndexInfoAccess {Database = database}};

            var tableInfo = tableInfoDao.GetInfo(config.TableCatalog, schemaName, tableName);

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
                return Cache[schemaName];
            }

            var database = Context.Get(Data.Database);

            var progress = Context.Get(Data.SchemaGenerationProcess);

            var tableNames = SchemaInfo.GetAllTableNamesInSchema(database, schemaName).ToList();

            tableNames = tableNames.Where(x => IsReadyToExport(schemaName, x)).ToList();

            var items = new List<ITableInfo>();

            progress.Total   = tableNames.Count;
            progress.Current = 0;

            foreach (var tableName in tableNames)
            {
                progress.Text = $"Fetching table information of {tableName}";
                progress.Current++;

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
            var config = Context.Get(Data.Config);

            var fullTableName = $"{schemaName}.{tableName}";

            var isNotExportable = config.NotExportableTables.Contains(fullTableName);
            if (isNotExportable)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}