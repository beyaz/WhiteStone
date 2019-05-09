﻿using System.Collections.Generic;
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

            foreach (var tableName in tableNames)
            {
                var tableInfo = TableInfoDao.GetInfo(TableCatalogName.BOACard, schemaName, tableName);

                var generatorData = GeneratorDataCreator.Create(tableInfo);

                items.Add(generatorData);
            }

            Cache[schemaName] = items;

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