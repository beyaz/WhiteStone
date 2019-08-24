using System.Collections.Generic;
using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class SchemaExporterDataPreparer
    {
        #region Fields
        static readonly Dictionary<string, IReadOnlyList<TableInfo>> Cache = new Dictionary<string, IReadOnlyList<TableInfo>>();
        #endregion

        #region Public Properties
        [Inject]
        public IDatabase Database { get; set; }

        [Inject]
        public GeneratorDataCreator GeneratorDataCreator { get; set; }

        [Inject]
        public TableInfoDao TableInfoDao { get; set; }

        [Inject]
        public Tracer Tracer { get; set; }

        
        [Inject]
        public Config Config { get; set; }
        
        #endregion



        #region Public Methods
        public IReadOnlyList<TableInfo> Prepare(string schemaName)
        {
            if (Cache.ContainsKey(schemaName))
            {
                Tracer.Trace($"Fetching from cache {schemaName}");
                return Cache[schemaName];
            }

            var tableNames = SchemaInfo.GetAllTableNamesInSchema(Database, schemaName).ToList();

            tableNames = tableNames.Where(x => IsReadyToExport(schemaName, x)).ToList();

            var items = new List<TableInfo>();


            Tracer.SchemaGenerationProcess.Total = tableNames.Count;
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

        public TableInfo GetTableInfo(string schemaName, string tableName)
        {
            var tableInfo = TableInfoDao.GetInfo(TableCatalogName.BOACard, schemaName, tableName);

            var generatorData = GeneratorDataCreator.Create(tableInfo);
            return generatorData;
        }
        #endregion

        #region Methods
         bool IsReadyToExport(string schemaName, string tableName)
         {
             var fullTableName = $"{schemaName}.{tableName}";

             return !Config.NotExportableTables.Contains(fullTableName);
         }
        #endregion
    }
}