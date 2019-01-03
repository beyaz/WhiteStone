using System.Linq;
using BOA.OneDesigner.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class CecilHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void GetAllBindProperties()
        {
            var bindProperties = CecilHelper.GetAllBindProperties(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ForeignDebitClearingRequest");

            Assert.IsTrue(bindProperties.Contains("FileId"));

            Assert.IsTrue(bindProperties.Contains("Account.IBAN"));
        }
        [TestMethod]
        public void GetAllBindPropertiesOfCollection()
        {
            var bindProperties = CecilHelper.GetAllBindProperties(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest","DataSource.DataGridRecords");

            Assert.IsTrue(bindProperties.Contains("Foreign.KBBusinessKey"));
        }
        
        #endregion
    }
}