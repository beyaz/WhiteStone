using System.IO;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModelDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.Generators
{
    [TestClass]
    public class ContractGenerationTest
    {
        #region Public Methods
        [TestMethod]
        public void ToString_method_should_evaluate_csharp_code_2()
        {
            ExportTypeToFile(@"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\Test\BOA.Process.Kernel.Card.Test\AddressTypeContract.cs", "PRM", "ADDRESS_TYPE");
            ExportTypeToFile(@"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\Test\BOA.Process.Kernel.Card.Test\CardLimitUsedContract.cs", "CRD", "CARD_LIMIT_USED");
        }
        #endregion

        #region Methods
        static void ExportTypeToFile(string filePath, string schemaName, string tableName)
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<Database>().To<BOACardDatabase>().InSingletonScope();
                kernel.Bind<IDatabase>().To<BOACardDatabase>().InSingletonScope();

                using (var database = kernel.Get<BOACardDatabase>())
                {
                    var tableInfoDao = kernel.Get<TableInfoDao>();

                    var contract = kernel.Get<Contract>();

                    var generatorData = kernel.Get<GeneratorDataCreator>().Create(tableInfoDao.GetInfo("BOACard", schemaName, tableName));

                    var contractData = kernel.Get<ContractDataCreator>().Create(generatorData);

                    var generatedCode = contract.TransformText(contractData);

                    File.WriteAllText(filePath, generatedCode);
                }
            }
        }
        #endregion
    }
}