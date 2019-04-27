using System.Collections.Generic;
using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class SchemaExporterDataPreparer
    {
        #region Public Properties
        [Inject]
        public Database Database { get; set; }

        [Inject]
        public GeneratorDataCreator GeneratorDataCreator { get; set; }

        [Inject]
        public TableInfoDao TableInfoDao { get; set; }
        #endregion

        #region Public Methods
        public virtual IReadOnlyList<TableInfo> Prepare(string schemaName)
        {
            var tableNames = SchemaInfo.GetAllTableNamesInSchema(Database, schemaName).ToList();

            tableNames = tableNames.Where(x => IsReadyToExport(schemaName, x)).ToList();

            var items = new List<TableInfo>();

            foreach (var tableName in tableNames)
            {
                var tableInfo = TableInfoDao.GetInfo(TableCatalogName.BOACard, schemaName, tableName);

                var generatorData = GeneratorDataCreator.Create(tableInfo);

                items.Add(generatorData);
            }

            return items;
        }
        #endregion

        #region Methods
        static bool IsReadyToExport(string schemaName, string tableName)
        {
            if ($"{schemaName}.{tableName}" == "MRC.SENDING_REPORT_PARAMS_")
            {
                return false;
            }

            if ($"{schemaName}.{tableName}" == "POS.POS_INVENTORY_")
            {
                return false;
            }

            if ($"{schemaName}.{tableName}" == "STM.BUCKET_CLOSE_DETAIL") // todo aynı indexden iki tane var ?  sormak lazım
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}