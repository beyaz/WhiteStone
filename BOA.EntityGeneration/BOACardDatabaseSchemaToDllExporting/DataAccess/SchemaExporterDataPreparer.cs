﻿using System.Linq;
using BOA.DataFlow;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public static class Events
    {
        #region Public Methods
        public static void OnSchemaStartedToExport(IDataContext context)
        {
            var database   = context.Get(Data.Database);
            var progress   = context.Get(Data.SchemaGenerationProcess);
            var schemaName = context.Get(Data.SchemaName);
            var config     = context.Get(Data.Config);

            var tableNames = SchemaInfo.GetAllTableNamesInSchema(database, schemaName).ToList();

            context.Add(Data.TableNamesInSchema, tableNames);

            context.FireEvent(DataEvent.AfterFetchedAllTableNamesInSchema);

            tableNames = context.Get(Data.TableNamesInSchema);

            context.Remove(Data.TableNamesInSchema);

            tableNames = tableNames.Where(x => IsReadyToExport(schemaName, x, config)).ToList();

            progress.Total   = tableNames.Count;
            progress.Current = 0;

            foreach (var tableName in tableNames)
            {
                progress.Text = $"Generating codes for table '{tableName}'.";
                progress.Current++;

                var tableInfoDao = new TableInfoDao {Database = database, IndexInfoAccess = new IndexInfoAccess {Database = database}};

                var tableInfo = tableInfoDao.GetInfo(config.TableCatalog, schemaName, tableName);

                var generatorData = GeneratorDataCreator.Create(config,database,tableInfo);

                context.Add(Data.TableInfo, generatorData);
                context.FireEvent(DataEvent.StartToExportTable);
                context.Remove(Data.TableInfo);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is ready to export] [the specified schema name].
        /// </summary>
        static bool IsReadyToExport(string schemaName, string tableName, Config config)
        {
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