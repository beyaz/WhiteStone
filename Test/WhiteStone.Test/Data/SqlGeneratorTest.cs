using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Data;
using WhiteStone.Data.Attributes;

namespace WhiteStone.Test.Data
{
    [DbSchema("AAA")]
    [DbTable("Customer")]
    class SqlGeneratorTestClass1
    {
        [DbColumn]
        public string P1 { get; set; }

        [DbColumn]
        [DbPrimaryKey]
        public string P2 { get; set; }

        [DbColumn]
        public int P3 { get; set; }

        [DbColumn]
        [DbPrimaryKey]
        public int? P4 { get; set; }

        [DbColumn]
        public decimal P5 { get; set; }

        [DbColumn]
        public decimal? P6 { get; set; }
    }


    [TestClass]
    public class SqlGeneratorTest
    {
        [TestMethod]
        public void GenerateInsertStatementFromEntityContract()
        {
            // ARRANGE
            var generator = new SqlGenerator();
            var contract = new SqlGeneratorTestClass1
            {
                P1 = "A",
                P2 = "B",
                P3 = 6,
                P4 = null,
                P5 = 56.78M,
                P6 = 89.43M
            };

            // ACT
            var result = generator.GenerateInsertStatementFromEntityContract(contract);

            // ASSERT
            var expectedSql = "INSERT INTO AAA.Customer (P1,P2,P3,P4,P5,P6) VALUES (@P1,@P2,@P3,@P4,@P5,@P6)";
            Assert.AreEqual(expectedSql, result.GenratedSql);

            Assert.AreEqual(6, result.GeneratedParameters.Count);
            Assert.AreEqual("P1", result.GeneratedParameters[0].ParameterName);
            Assert.AreEqual("P2", result.GeneratedParameters[1].ParameterName);
            Assert.AreEqual("P3", result.GeneratedParameters[2].ParameterName);
            Assert.AreEqual("P4", result.GeneratedParameters[3].ParameterName);
            Assert.AreEqual("P5", result.GeneratedParameters[4].ParameterName);
            Assert.AreEqual("P6", result.GeneratedParameters[5].ParameterName);


            Assert.AreEqual("A", result.GeneratedParameters[0].Value);
            Assert.AreEqual("B", result.GeneratedParameters[1].Value);
            Assert.AreEqual(6, result.GeneratedParameters[2].Value);
            Assert.AreEqual(null, result.GeneratedParameters[3].Value);
            Assert.AreEqual(56.78M, result.GeneratedParameters[4].Value);
            Assert.AreEqual(89.43M, result.GeneratedParameters[5].Value);
        }


        [TestMethod]
        public void GenerateSelectStatementFromEntityContract()
        {
            // ARRANGE
            var generator = new SqlGenerator();
            var contract = new SqlGeneratorTestClass1
            {
                P1 = "A",
                P2 = "B",
                P3 = 6,
                P4 = 67,
                P5 = 56.78M,
                P6 = 89.43M
            };

            // ACT
            var result = generator.GenerateSelectStatementFromEntityContract(contract);

            // ASSERT
            var expectedSql = "SELECT * FROM AAA.Customer WHERE P2 = @P2,P4 = @P4";
            Assert.AreEqual(expectedSql, result.GenratedSql);

            Assert.AreEqual(2, result.GeneratedParameters.Count);
            Assert.AreEqual("P2", result.GeneratedParameters[0].ParameterName);
            Assert.AreEqual("P4", result.GeneratedParameters[1].ParameterName);


            Assert.AreEqual("B", result.GeneratedParameters[0].Value);
            Assert.AreEqual(67, result.GeneratedParameters[1].Value);
        }


        [TestMethod]
        public void GenerateDeleteStatementFromEntityContract()
        {
            // ARRANGE
            var generator = new SqlGenerator();
            var contract = new SqlGeneratorTestClass1
            {
                P1 = "A",
                P2 = "B",
                P3 = 6,
                P4 = 67,
                P5 = 56.78M,
                P6 = 89.43M
            };

            // ACT
            var result = generator.GenerateDeleteStatementFromEntityContract(contract);

            // ASSERT
            var expectedSql = "DELETE FROM AAA.Customer WHERE P2 = @P2,P4 = @P4";
            Assert.AreEqual(expectedSql, result.GenratedSql);

            Assert.AreEqual(2, result.GeneratedParameters.Count);
            Assert.AreEqual("P2", result.GeneratedParameters[0].ParameterName);
            Assert.AreEqual("P4", result.GeneratedParameters[1].ParameterName);


            Assert.AreEqual("B", result.GeneratedParameters[0].Value);
            Assert.AreEqual(67, result.GeneratedParameters[1].Value);
        }


        [TestMethod]
        public void GenerateUpdateStatementFromEntityContract()
        {
            // ARRANGE
            var generator = new SqlGenerator();
            var contract = new SqlGeneratorTestClass1
            {
                P1 = "A",
                P2 = "B",
                P3 = 6,
                P4 = 67,
                P5 = 56.78M,
                P6 = 89.43M
            };

            // ACT
            var result = generator.GenerateUpdateStatementFromEntityContract(contract);

            // ASSERT
            var expectedSql = "UPDATE AAA.Customer SET P1 = @P1,P3 = @P3,P5 = @P5,P6 = @P6 WHERE P2 = @P2,P4 = @P4";
            Assert.AreEqual(expectedSql, result.GenratedSql);

            Assert.AreEqual(6, result.GeneratedParameters.Count);
            Assert.AreEqual("P1", result.GeneratedParameters[0].ParameterName);
            Assert.AreEqual("P3", result.GeneratedParameters[1].ParameterName);
            Assert.AreEqual("P5", result.GeneratedParameters[2].ParameterName);
            Assert.AreEqual("P6", result.GeneratedParameters[3].ParameterName);

            Assert.AreEqual("P2", result.GeneratedParameters[4].ParameterName);
            Assert.AreEqual("P4", result.GeneratedParameters[5].ParameterName);


            Assert.AreEqual("A", result.GeneratedParameters[0].Value);
            Assert.AreEqual(6, result.GeneratedParameters[1].Value);
            Assert.AreEqual(56.78M, result.GeneratedParameters[2].Value);
            Assert.AreEqual(89.43M, result.GeneratedParameters[3].Value);

            Assert.AreEqual("B", result.GeneratedParameters[4].Value);
            Assert.AreEqual(67, result.GeneratedParameters[5].Value);
        }
    }
}