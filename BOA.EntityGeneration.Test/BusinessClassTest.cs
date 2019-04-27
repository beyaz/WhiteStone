using System.IO;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using BOA.EntityGeneration.DbModelDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.Generators
{
    [TestClass]
    public class BusinessClassTest
    {
        #region Public Methods
        [TestMethod]
        public void Generate()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<Database>().To<BOACardDatabase>().InSingletonScope();
                kernel.Bind<IDatabase>().To<BOACardDatabase>().InSingletonScope();

                using (var database = kernel.Get<BOACardDatabase>())
                {
                    var tableInfoDao = kernel.Get<TableInfoDao>();

                    var businessClass = kernel.Get<GeneratorOfBusinessClass>();

                    var generatorData = kernel.Get<GeneratorDataCreator>().Create(tableInfoDao.GetInfo(TableCatalogName.BOACard, "CRD", "CARD_LIMIT_USED"));

                    var generatedCode = businessClass.TransformText(generatorData);

                    File.WriteAllText(@"D:\CardLimitUsedContract.cs", generatedCode);
                }
            }
        }
        #endregion
    }
}