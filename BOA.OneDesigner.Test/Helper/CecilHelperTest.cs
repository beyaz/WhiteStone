using BOA.OneDesigner.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.Helper
{
    [TestClass]
    public class CecilHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void GetAllBindProperties()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ForeignDebitClearingRequest");

            data.RequestPropertyIntellisense.Should().Contain("FileId");
            data.RequestPropertyIntellisense.Should().Contain("Account.IBAN");
        }

        [TestMethod]
        public void GetAllBindPropertiesOfCollection()
        {
            var bindProperties = CecilHelper.GetAllBindProperties(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest", "DataSource.DataGridRecords");

            bindProperties.Should().Contain("Foreign.KBBusinessKey");
        }

    

        [TestMethod]
        public void GetPropertyInfo_should_get_cecil_property_info()
        {
            var definition = CecilHelper.FindPropertyInfo(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest", "Data.AccountNumber");

            definition.Name.Should().Be("AccountNumber");
        }
        #endregion
    }
}