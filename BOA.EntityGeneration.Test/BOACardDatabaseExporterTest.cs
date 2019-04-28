using System.Collections.Generic;
using System.Data;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{

  
    [TestClass]
    public class BOACardDatabaseExporterTest
    {


        [TestMethod]
        public void CustomSqlInjection()
        {
            using (var kernel = new TestKernel())
            {
                kernel.Get<ProjectInjector>().Inject("CC_OPERATIONS");
            }
        }

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
                Bind<SchemaExporterDataPreparer>().To<SchemaExporterDataPreparerFromLocalJsonFile>();
            }
            #endregion
        }
    }
}