using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGeneration;
using BOA.OneDesigner.MainForm;
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
            DevelopmentDatabase database = new DevelopmentDatabase();

            var screenInfo = database.GetScreenInfo("BOA.Types.CardGeneral.DebitCard.MccDefinitionFormRequest");

            Controller.Generate(screenInfo);
            
        }
        #endregion
    }
}