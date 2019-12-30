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
        public void GetAllBindProperties_should_contains_collection_properties()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.CardTransactionListFormRequest");

            data.RequestPropertyIntellisense.Should().Contain("Data.ExternalResponseCodes");
        }

        [TestMethod]
        public void PropertyHasAttribute()
        {
            var data = CecilHelper.GetAttributeAttachedPropertyNames(@"d:\boa\server\bin\BOA.Types.Card.Clearing.Visa.dll", "BOA.Types.Card.Clearing.Visa.GetVisaIssuerByCustomCriteriaRequest","DoNotSendToServerFromClientAttribute");

            data.Should().Contain("Records");
        }



        [TestMethod]
        public void GetAllBindPropertiesOfCollection()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest");

            data.Collections.Should().ContainKey("DataSource.DataGridRecords");
            data.Collections["DataSource.DataGridRecords"].Should().Contain("Foreign.KBBusinessKey");

            var info = data.FindPropertyInfoInCollectionFirstGenericArgumentType("DataSource.DataGridRecords", "Foreign.KBBusinessKey");
            info.Should().NotBeNull();
        }

        [TestMethod]
        public void GetPropertyInfo_should_get_cecil_property_info()
        {
            var definition = CecilHelper.FindPropertyInfo(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ChargebackListFormRequest", "Data.AccountNumber");

            definition.Name.Should().Be("AccountNumber");
        }

        [TestMethod]
        public void Should_evaluate_orchestration_methods()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.CardTransactionListFormRequest");

            data.OrchestrationMethods.Should().Contain("LoadData");
        }

        [TestMethod]
        public void Should_handle_long_properties()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.Card.Clearing.Visa.dll", "BOA.Types.Card.Clearing.Visa.GetVisaIssuerByCustomCriteriaRequest");

            data.RequestPropertyIntellisense.Should().Contain("ClearingNumber");
        }

        [TestMethod]
        public void Should_handle_circular_properties()
        {
            var data = CecilHelper.GetRequestIntellisenseData(@"d:\boa\server\bin\BOA.Types.Kernel.Customer.dll", "BOA.Types.Kernel.Customer.CustomerInfoContract");

            data.RequestPropertyIntellisense.Should().Contain("ClearingNumber");
        }
        #endregion
    }
}