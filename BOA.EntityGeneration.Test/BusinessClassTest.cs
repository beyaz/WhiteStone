using System.IO;
using BOA.EntityGeneration.DbModelDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.Generators
{
    [TestClass]
    public class BusinessClassTest
    {
        #region Public Methods
        [TestMethod]
        public void Generate()
        {
            using (var database = new BOACardDatabase())
            {
                var dao = new TableInfoDao
                {
                    Database = database
                };

                var contract = new BusinessClass
                {
                    Data = GeneratorDataCreator.Create(dao.GetInfo("BOACard", "CRD", "CARD_LIMIT_USED"), database)
                };

                File.WriteAllText(@"D:\CardLimitUsedContract.cs", contract.TransformText());
            }
        }
        #endregion
    }
}