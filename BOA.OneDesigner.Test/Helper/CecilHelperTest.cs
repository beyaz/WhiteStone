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
            data.RequestPropertyIntellisense.Should().Contain("Account");
        }

        [TestMethod]
        public void Should_evaluate_orhestration_methods()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.CardTransactionListFormRequest");

            data.OrchestrationMethods.Should().Contain("LoadData");
        }
        

        [TestMethod]
        public void GetAllBindPropertiesOfCollection()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest");

            data.Collections.Should().ContainKey("DataSource.DataGridRecords");
            data.Collections["DataSource.DataGridRecords"].Should().Contain("Foreign.KBBusinessKey");

            CecilPropertyInfo info= data.FindPropertyInfoInCollectionFirstGenericArgumentType("DataSource.DataGridRecords", "Foreign.KBBusinessKey");
            info.Should().NotBeNull();

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