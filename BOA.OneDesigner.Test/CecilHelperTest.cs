using BOA.OneDesigner.Helpers;
using FluentAssertions;
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

            bindProperties.Should().Contain("FileId");
            bindProperties.Should().Contain("Account.IBAN");
        }

        [TestMethod]
        public void GetAllBindPropertiesOfCollection()
        {
            var bindProperties = CecilHelper.GetAllBindProperties(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest", "DataSource.DataGridRecords");

            bindProperties.Should().Contain("Foreign.KBBusinessKey");
        }
        #endregion
    }
}