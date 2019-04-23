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
        public Func<string,string,bool> TableNameFilter { get; set; }
        public IReadOnlyList<string> ReferencedAssemblies { get; set; }
        #endregion
    }

    public class SchemaExporterDataForBOACard : SchemaExporterData
    {
        public SchemaExporterDataForBOACard()
        {
            CatalogName = "BOACard";
            TableNameFilter =  IsReadyToExport;
            ReferencedAssemblies = new List<string>
            {
                @"d:\boa\server\bin\BOA.Types.Kernel.Card.dll",
                @"d:\boa\server\bin\BOA.Common.dll"
            };
        }

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
                tableNames = tableNames.Where(x=> data.TableNameFilter(data.SchemaName,x)).ToList();
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
                OutputAssemblyName = data.SchemaName,
                Sources = sources.ToArray(),
                ReferencedAssemblies = data.ReferencedAssemblies
            };

            compiler.Compile();
        }
        #endregion
    }
}