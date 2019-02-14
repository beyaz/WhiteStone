using System;
using System.IO;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var data = new ScreenInfo
            {
                RequestName = "A,b",
                JsxModel    = new ScreenInfo {RequestName = "Aloha"}
            };

            database.Save(data);
            database.Save(data);
            database.Save(data);

            var dataInDb = new ScreenInfo {RequestName = data.RequestName};
            database.Load(dataInDb);

            (dataInDb.JsxModel as ScreenInfo)?.RequestName.Should().Be("Aloha");

            database.DeleteByRequestName(data.RequestName).Should().Be(1);
        }

        [TestMethod]
        public void SaveToFile()
        {
            var database = new DevelopmentDatabase();

            var path = nameof(SaveToFile) + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + ".bin";
            File.WriteAllBytes(path, BinarySerialization.Serialize(database.GetAllScreens()));
        }

        [TestMethod]
        public void VisitAllAndConvertToAccountComponent()
        {
            var database = new DevelopmentDatabase();

            foreach (var screenInfo in database.GetAllScreens())
            {
                VisitHelper.VisitAllChildren(screenInfo, VisitHelper.ConvertToNewComponent);
            }
        }
        #endregion
    }
}