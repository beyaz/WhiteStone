using System.IO;
using BOA.EntityGeneration.DbModelDao;
using BOA.EntityGeneration.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    [TestClass]
    public class ContractGenerationTest
    {



        [TestMethod]
        public void ToString_method_should_evaluate_csharp_code_2()
        {
            using (var database = new BOACardDatabase())
            {
                var dao = new TableInfoDao
                {
                    Database = database
                };

                var contract = new Contract
                {
                    Data = GeneratorDataCreator.Create(dao.GetInfo("BOACard", "CRD", "CARD_LIMIT_USED"))
                };

                File.WriteAllText(@"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\Test\BOA.Process.Kernel.Card.Test\CardLimitUsedContract.cs", contract.ToString());


                contract = new Contract
                {
                    Data = GeneratorDataCreator.Create(dao.GetInfo("BOACard", "PRM", "ADDRESS_TYPE"))
                };

                File.WriteAllText(@"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\Test\BOA.Process.Kernel.Card.Test\AddressTypeContract.cs", contract.ToString());
            }
        }
    }
}