using System;
using System.Collections.Generic;
using System.Linq;
using BOA.CodeGeneration.Contracts.Dao;
using BOA.DatabaseAccess;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class SchemaExporterData
    {
        #region Public Properties
        public string                CatalogName        { get; set; }
        public IDatabase             Database           { get; set; }
        public Action<GeneratorData> OnTableDataCreated { get; set; }
        public string                SchemaName         { get; set; }
        public Func<string,bool> TableNameFilter { get; set; }
        #endregion
    }

    public static class SchemaExporter
    {
        #region Public Methods
        public static void Export(SchemaExporterData data)
        {
            var sources = new List<string>();

            var tableNames = SchemaInfoDao.GetAllTableNamesInSchema(data.Database, data.SchemaName).ToList();

            if (data.TableNameFilter != null)
            {
                tableNames = tableNames.Where(data.TableNameFilter).ToList();
            }
            

            foreach (var tableName in tableNames)
            {
                var dao = new TableInfoDao
                {
                    Database = data.Database
                };

                var tableInfo = dao.GetInfo(data.CatalogName, data.SchemaName, tableName);

                var contract = new Contract
                {
                    Data = GeneratorDataCreator.Create(tableInfo)
                };

                data.OnTableDataCreated?.Invoke(contract.Data);

                sources.Add(contract.ToString());
            }

            var compiler = new Compiler
            {
                OutputAssemblyName = data.SchemaName, Sources = sources.ToArray()
            };

            compiler.Compile();
        }
        #endregion
    }
}