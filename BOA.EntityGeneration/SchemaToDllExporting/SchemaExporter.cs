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
        public Compiler Compiler { get; set; }

        [Inject]
        public Database Database { get; set; }

        [Inject]
        public GeneratorDataCreator GeneratorDataCreator { get; set; }

        [Inject]
        public TableInfoDao TableInfoDao { get; set; }
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
                var tableInfo = TableInfoDao.GetInfo(data.CatalogName, data.SchemaName, tableName);

                var contract = new Contract
                {
                    Data = GeneratorDataCreator.Create(tableInfo)
                };

                data.OnTableDataCreated?.Invoke(contract.Data);

                sources.Add(contract.ToString());
            }

            Compiler.Compile(new CompilerData
            {
                OutputAssemblyName   = data.SchemaName,
                Sources              = sources.ToArray(),
                ReferencedAssemblies = data.ReferencedAssemblies
            });
        }
        #endregion
    }
}