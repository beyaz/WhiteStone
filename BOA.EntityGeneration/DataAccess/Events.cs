using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;

namespace BOA.EntityGeneration.DataAccess
{
    public static class Events
    {
        #region Public Methods
        public static void OnSchemaStartedToExport(IContext context)
        {
            var database   = context.Get(Data.Database);
            var progress   = context.Get(Data.ProcessInfo);
            var schemaName = context.Get(Data.SchemaName);
            var config     = context.Get(Data.Config);

            var tableNames = SchemaInfo.GetAllTableNamesInSchema(database, schemaName).ToList();

            context.OpenBracket();
            context.Add(Data.TableNamesInSchema, tableNames);

            context.FireEvent(DataEvent.AfterFetchedAllTableNamesInSchema);

            tableNames = context.Get(Data.TableNamesInSchema);

            context.CloseBracket();

            tableNames = tableNames.Where(x => IsReadyToExport(schemaName, x, config)).ToList();

            progress.Total   = tableNames.Count;
            progress.Current = 0;

            foreach (var tableName in tableNames)
            {
                progress.Text = $"Generating codes for table '{tableName}'.";
                progress.Current++;

                var tableInfoDao = new TableInfoDao {Database = database, IndexInfoAccess = new IndexInfoAccess {Database = database}};

                var tableInfo = tableInfoDao.GetInfo(config.TableCatalog, schemaName, tableName);

                var generatorData = GeneratorDataCreator.Create(config.SqlSequenceInformationOfTable, config.DatabaseEnumName, database, tableInfo);

                context.OpenBracket();
                context.Add(Data.TableInfo, generatorData);
                context.FireEvent(TableExportingEvent.TableExportStarted);
                context.FireEvent(TableExportingEvent.TableExportFinished);
                context.CloseBracket();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is ready to export] [the specified schema name].
        /// </summary>
        static bool IsReadyToExport(string schemaName, string tableName, ConfigContract config)
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