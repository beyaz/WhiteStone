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
        public void GenerateAll()
        {
            var database = new DevelopmentDatabase();

            foreach (var screen in database.GetAllScreens())
            {
                Controller.Generate(screen);
            }
        }
        #endregion
    }
}