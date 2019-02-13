using System;
using System.Linq;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
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
            using (var database = new DevelopmentDatabase())
            {
                var screens = database.GetAllScreens();

                screens = screens.Where(x => x.UserName == Environment.UserName).ToList();

                foreach (var screen in screens)
                {
                    

                     VisitHelper.VisitAllChildren(screen, VisitHelper.ConvertToAccountComponent);
                    Controller.Generate(screen);
                }
            }
        }
        #endregion
    }
}