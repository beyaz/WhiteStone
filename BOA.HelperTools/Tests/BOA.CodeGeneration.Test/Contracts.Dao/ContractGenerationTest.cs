using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    [TestClass]
    public class ContractGenerationTest
    {
        [TestMethod]
        public void ToString_method_should_evaluate_csharp_code()
        {
            using (var database = new BOACardDatabase())
            {
                var dao = new TableInfoDao
                {
                    Database = database
                };

                var tableInfo = dao.GetInfo("BOACard","PRM","ADDRESS_TYPE");

                var contract = new Transforms.Contract
                {
                    TableInfo = tableInfo
                };

                File.WriteAllText("d:\\A.cs",contract.ToString());
                
            }

            
        }
    }
}