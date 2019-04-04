using System;
using System.Collections.Generic;
using System.Linq;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.MainForm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class TransactionPageTest
    {
        #region Public Methods
        [TestMethod]
        public void Card360()
        {
            
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
                    catch (BusinessException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        [TestMethod]
        public void Debug()
        {
            
            using (var database = new DevelopmentDatabase())
            {
                var screens = database.GetAllScreens();

                var files = new[] {"card-agriculture-profile-restriction-list","card360-edit","card360","DebitParameterForm","GeneralParametersForm"};

                screens = screens.Where(x => files.Contains(x.OutputTypeScriptFileName)).ToList();

                foreach (var screen in screens)
                {
                    Controller.Generate(screen);
                }
            }
        }

        [TestMethod]
        public void GenerateAll()
        {
            var excepts = new List<string>();
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