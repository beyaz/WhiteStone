using System.Collections.Generic;
using System.Linq;
using BOA.EntityGeneration.DbModelDao;
using BOA.EntityGeneration.Generators;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public static class SchemaExporter
    {
        #region Public Methods
        public static void Export(SchemaExporterData data)
        {
            var sources = new List<string>();

            var tableNames = SchemaInfoDao.GetAllTableNamesInSchema(data.Database, data.SchemaName).ToList();

            if (data.TableNameFilter != null)
            {
                tableNames = tableNames.Where(x => data.TableNameFilter(data.SchemaName, x)).ToList();
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
                    Data = GeneratorDataCreator.Create(tableInfo, data.Database)
                };

                data.OnTableDataCreated?.Invoke(contract.Data);

                sources.Add(contract.ToString());
            }

            var compiler = new Compiler
            {
                OutputAssemblyName   = data.SchemaName,
                Sources              = sources.ToArray(),
                ReferencedAssemblies = data.ReferencedAssemblies
            };

            compiler.Compile();
        }
        #endregion
    }
}