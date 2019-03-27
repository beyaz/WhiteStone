using System;
using System.Collections.Generic;
using System.Linq;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.MainForm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exception = System.Exception;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class TransactionPageTest
    {

        [TestMethod]
        public void Card360()
        {
            var excepts = new List<string>
            {
            };
            using (var database = new DevelopmentDatabase())
            {
                var screens = database.GetAllScreens();

                screens = screens.Where(x => x.RequestName.Contains("360")).ToList();

                foreach (var screen in screens)
                {
                    try
                    {
                        Controller.Generate(screen);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        #region Public Methods
        [TestMethod]
        public void GenerateAll()
        {
            var excepts = new List<string>
            {
            };
            using (var database = new DevelopmentDatabase())
            {
                var screens = database.GetAllScreens();

                screens = screens.Where(x => excepts.Contains(x.RequestName) == false).ToList();

                foreach (var screen in screens)
                {
                    try
                    {
                        Controller.Generate(screen);
                    }
                    catch (BusinessException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        #endregion
    }
}