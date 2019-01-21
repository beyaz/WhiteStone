using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BOA.OneDesigner.AppModel
{
    [TestClass]
    public class DevelopmentDatabaseTest
    {

        [TestMethod]
        public void UpdateDataModel()
        {
            var database = new DevelopmentDatabase();


            foreach (var screenInfo in database.GetAllScreens())
            {
                var divAsCardContainer =  screenInfo.JsxModel as DivAsCardContainer;
                VisitAll(divAsCardContainer, (field) =>
                {
                    ((BComponent) field).ValueBindingPath = field.ValueBindingPath;
                });

                database.Save(screenInfo);
            }
        }

        static void VisitAll(DivAsCardContainer divAsCardContainer, Action<BField> action)
        {
            if (divAsCardContainer == null)
            {
                return;
            }
            foreach (var bCard in divAsCardContainer.Items)
            {
                foreach (var field in bCard.Items)
                {
                    action(field);
                }
            }
        }





        #region Public Methods
        [TestMethod]
        public void Save_should_be_insert_or_update_database()
        {
            var database = new DevelopmentDatabase();

            var screenInfos = database.GetAllScreens();


            var data = new ScreenInfo
            {
                RequestName = "A,b",
                JsxModel = new ScreenInfo { RequestName = "Aloha" }
            };

            database.Save(data);
            database.Save(data);
            database.Save(data);

            var dataInDb = new ScreenInfo { RequestName = data.RequestName };
            database.Load(dataInDb);

            (dataInDb.JsxModel as ScreenInfo)?.RequestName.Should().Be("Aloha");

            //var jsonFile = new JsonFile();
            //data= jsonFile.GetScreenInfo("BOA.Types.CardGeneral.DebitCard.RestrictedMCCRequest");
            //database.Save(data);


            //foreach (var screenInfo in database.GetAllScreens())
            //{
            //    if (screenInfo.ResourceActions == null)
            //    {
            //        continue;
            //    }
            //    foreach (var resourceAction in screenInfo.ResourceActions)
            //    {
            //        resourceAction.IsVisibleBindingPath = resourceAction.IsEnabledBindingPath;

            //        resourceAction.IsEnabledBindingPath = null;
            //    }

            //    database.Save(screenInfo);
            //}
        }


        #endregion
    }
}