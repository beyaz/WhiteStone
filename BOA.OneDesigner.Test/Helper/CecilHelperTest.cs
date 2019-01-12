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

        [TestMethod]
        public void GetAllBooleanBindProperties()
        {
            var bindProperties = CecilHelper.GetAllBooleanBindProperties(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ForeignDebitClearingRequest");

            bindProperties.Should().Contain("Account.HasKMH");
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