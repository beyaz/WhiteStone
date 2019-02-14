using System;
using System.Collections.Generic;
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
            var excepts = new List<string>
            {
                "BOA.Types.Card.Parameter.InboundTranMappingRequest",// göknur 
                "BOA.Types.Card.Switch.BKM.GetTransactionLogDetailRequest",// fidan 
            };
            using (var database = new DevelopmentDatabase())
            {
                var screens = database.GetAllScreens();

                screens = screens.Where(x => excepts.Contains(x.RequestName) == false).ToList();

                foreach (var screen in screens)
                {
                    Controller.Generate(screen);
                }
            }
        }
        #endregion
    }
}