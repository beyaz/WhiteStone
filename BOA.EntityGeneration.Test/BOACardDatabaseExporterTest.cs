using System.Collections.Generic;
using System.Data;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void Export()
        {
            using (var kernel = new TestKernel())
            {
                BOACardDatabaseExporter.Export(kernel);
            }
        }
        #endregion

        class SchemaExporterDataPreparerFromLocalJsonFile : SchemaExporterDataPreparer
        {
            #region Public Methods
            public override IReadOnlyList<TableInfo> Prepare(string schemaName)
            {
                var filePath = schemaName + ".json";

                if (File.Exists(filePath))
                {
                    return JsonHelper.Deserialize<IReadOnlyList<TableInfo>>(File.ReadAllText(filePath));
                }

                var data = base.Prepare(schemaName);

                FileHelper.WriteAllText(filePath, JsonHelper.Serialize(data));

                return data;
            }
            #endregion
        }

        class TestKernel : Kernel
        {
            #region Constructors
            public TestKernel()
            {
                Unbind<IDatabase>();
                Bind<IDatabase>().To<InvalidDb>();
                Bind<SchemaExporterDataPreparer>().To<SchemaExporterDataPreparerFromLocalJsonFile>();
            }
            #endregion
        }

        public class InvalidDb:IDatabase
        {
            public void Dispose()
            {
               
            }

            public void BeginTransaction()
            {
                throw new System.NotImplementedException();
            }

            public void Commit()
            {
                throw new System.NotImplementedException();
            }

            public int ExecuteNonQuery()
            {
                throw new System.NotImplementedException();
            }

            public IDataReader ExecuteReader()
            {
                throw new System.NotImplementedException();
            }

            public object ExecuteScalar()
            {
                throw new System.NotImplementedException();
            }

            public void Rollback()
            {
                throw new System.NotImplementedException();
            }

            public bool CommandIsStoredProcedure { get; set; }
            public string CommandText { get; set; }
            public int? CommandTimeout { get; set; }
            public string ParameterPrefix { get; }

            public object this[string parameterName]
            {
                get => throw new System.NotImplementedException();
                set => throw new System.NotImplementedException();
            }
        }
    }
}