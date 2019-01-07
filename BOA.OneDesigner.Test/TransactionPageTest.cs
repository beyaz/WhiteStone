using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGeneration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class TransactionPageTest
    {
        #region Public Methods
        [TestMethod]
        public void Generate()
        {
            var host       = new Host();
            var screenInfo = host.Database.GetScreenInfo("BOA.Types.CardGeneral.DebitCard.GeneralParametersFormRequest");

            var tsxCode = TransactionPage.Generate(screenInfo);

            tsxCode.Should().NotBeNullOrEmpty();
        }
        #endregion
    }
}