using BOA.OneDesigner.CodeGeneration;
using BOAPlugins.Utility;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Host = BOA.OneDesigner.AppModel.Host;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class TransactionPageTest
    {
        #region Public Methods
        [TestMethod]
        public void Generate()
        {
            var host = new Host();

            var screenInfo = host.Database.GetScreenInfo("BOA.Types.CardGeneral.DebitCard.MccDefinitionFormRequest");

            var tsxCode = TransactionPage.Generate(screenInfo);

            tsxCode.Should().NotBeNullOrEmpty();

            Util.WriteFileIfContentNotEqual(@"D:\Work\BOA.BusinessModules\Dev\BOA.CardGeneral.DebitCard\One\BOA.One.CardGeneral.DebitCard\ClientApp\pages\MccDefinitionForm.tsx", tsxCode);
        }
        #endregion
    }
}