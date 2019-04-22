using System;
using System.Collections.Generic;
using System.Linq;
using BOA.CodeGeneration.Contracts.Dao;
using BOA.DatabaseAccess;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public static class Exporter
    {
        #region Public Methods
        public static void ExportAll(IDatabase database)
        {
        }

        public static void ExportSchema(IDatabase database, string catalogName, string schema, Action<GeneratorData> onTableDataCreated)
        {
            var sources = new List<string>();

            var tableNames = SchemaInfoDao.GetAllTableNamesInSchema(database, schema);

            foreach (var tableName in tableNames)
            {
                var dao = new TableInfoDao
                {
                    Database = database
                };

                var tableInfo = dao.GetInfo(catalogName, schema, tableName);

                var contract = new Contract
                {
                    Data = GeneratorDataCreator.Create(tableInfo)
                };

                onTableDataCreated?.Invoke(contract.Data);

                sources.Add(contract.ToString());
            }

            var compiler = new Compiler
            {
                OutputAssemblyName = schema, Sources = sources.ToArray()
            };

            compiler.Compile();
        }
        #endregion
    }
}