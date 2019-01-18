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


        #region Public Methods
        [TestMethod]
        public void Save_should_be_insert_or_update_database()
        {
            var database = new DevelopmentDatabase();


            File.WriteAllBytes($"d:\\temp\\{nameof(database.GetAllScreens)}.binary", BinarySerialization.Serialize(database.GetAllScreens()));
            File.WriteAllBytes($"d:\\temp\\{nameof(database.GetTfsFolderNames)}.binary", BinarySerialization.Serialize(database.GetTfsFolderNames()));
            File.WriteAllBytes($"d:\\temp\\{nameof(database.GetDefaultRequestNames)}.binary", BinarySerialization.Serialize(database.GetDefaultRequestNames()));



            //var data = new ScreenInfo
            //{
            //    RequestName = "A,b",
            //    JsxModel = new ScreenInfo { RequestName = "Aloha" }
            //};

            //database.Save(data);
            //database.Save(data);
            //database.Save(data);

            //var dataInDb = new ScreenInfo { RequestName = data.RequestName };
            //database.Load(dataInDb);

            //(dataInDb.JsxModel as ScreenInfo)?.RequestName.Should().Be("Aloha");

            ////var jsonFile = new JsonFile();
            ////data= jsonFile.GetScreenInfo("BOA.Types.CardGeneral.DebitCard.RestrictedMCCRequest");
            ////database.Save(data);


            ////foreach (var screenInfo in database.GetAllScreens())
            ////{
            ////    if (screenInfo.ResourceActions == null)
            ////    {
            ////        continue;
            ////    }
            ////    foreach (var resourceAction in screenInfo.ResourceActions)
            ////    {
            ////        resourceAction.IsVisibleBindingPath = resourceAction.IsEnabledBindingPath;

            ////        resourceAction.IsEnabledBindingPath = null;
            ////    }

            ////    database.Save(screenInfo);
            ////}
        }


        #endregion
    }
}