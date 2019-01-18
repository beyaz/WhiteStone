using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.MainForm;
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
            var database = new DevelopmentDatabase();

            var screenInfo = database.GetScreenInfo("BOA.Types.Card.Parameter.DppKeyFormRequest");

            Controller.Generate(screenInfo);
        }
        #endregion
    }
}