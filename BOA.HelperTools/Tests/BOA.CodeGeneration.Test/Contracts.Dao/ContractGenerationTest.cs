using System.IO;
using BOA.CodeGeneration.Contracts.Transforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    [TestClass]
    public class ContractGenerationTest
    {
        #region Public Methods
        [TestMethod]
        public void ToString_method_should_evaluate_csharp_code()
        {
            using (var database = new TestDatabase())
            {
                var dao = new TableInfoDao
                {
                    Database = database
                };

                var tableInfo = dao.GetInfo(TestDatabase.CatalogName, "TST", "Table1");

                var contract = new Contract
                {
                    TableInfo = tableInfo
                };

                File.WriteAllText("d:\\A.cs", contract.ToString());
            }
        }

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
                    TableInfo = dao.GetInfo("BOACard", "CRD", "CARD_LIMIT_USED")
                };

                File.WriteAllText("d:\\B.cs", contract.ToString());
            }
        }
        #endregion
    }
}