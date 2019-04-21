using System.IO;
using BOA.CodeGeneration.Contracts.Transforms;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    public class TestDatabase : SqlDatabase
    {
        const string ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\github\WhiteStone\BOA.HelperTools\Tests\BOA.CodeGeneration.Test\Database1.mdf; Integrated Security = True";
        public TestDatabase():base(ConnectionString)
        {
            
        }
    }

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

                var tableInfo = dao.GetInfo("testDB", "TST", "Table1");

                var contract = new Contract
                {
                    TableInfo = tableInfo
                };

                File.WriteAllText("d:\\A.cs", contract.ToString());




                contract = new Contract
                {
                    TableInfo = dao.GetInfo("BOACard", "CRD", "CARD_LIMIT_USED")
                };

                File.WriteAllText("d:\\B.cs", contract.ToString());


            }
        }
        #endregion
    }
}