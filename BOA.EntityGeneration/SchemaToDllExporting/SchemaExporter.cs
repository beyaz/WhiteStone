using System.Collections.Generic;
using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModelDao;
using BOA.EntityGeneration.Generators;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public Database Database { get; set; }
        #endregion

        #region Public Methods
        public void Export(SchemaExporterData data)
        {
            var sources = new List<string>();

            var tableNames = SchemaInfoDao.GetAllTableNamesInSchema(Database, data.SchemaName).ToList();

            if (data.TableNameFilter != null)
            {
                tableNames = tableNames.Where(x => data.TableNameFilter(data.SchemaName, x)).ToList();
            }

            foreach (var tableName in tableNames)
            {
                var dao = new TableInfoDao
                {
                    Database = Database
                };

                var tableInfo = dao.GetInfo(data.CatalogName, data.SchemaName, tableName);

                var contract = new Contract
                {
                    Data = GeneratorDataCreator.Create(tableInfo, Database)
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