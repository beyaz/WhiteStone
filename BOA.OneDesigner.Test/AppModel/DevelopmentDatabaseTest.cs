using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.AppModel
{
    [TestClass]
    public class DevelopmentDatabaseTest
    {

       

        #region Public Methods
        [TestMethod]
        public void GetAndSaveAllToDb()
        {
            using (var database = new DevelopmentDatabase())
            {
                foreach (var screenInfo in database.GetAllScreens())
                {
                    database.Save(screenInfo);
                }
            }
        }

        [TestMethod]
        public void Save_should_be_insert_or_update_database()
        {
            using (var database = new DevelopmentDatabase())
            {
                var data = new ScreenInfo
                {
                    RequestName = "A,b",
                    JsxModel    = new DivAsCardContainer() { Items = new List<BCard>{ new BCard{ TitleInfo = new LabelInfo{ FreeTextValue = "Aloha",IsFreeText = true}}}}
                };

                database.Save(data);
                database.Save(data);
                database.Save(data);

                var dataInDb = new ScreenInfo {RequestName = data.RequestName};
                database.Load(dataInDb);

                ((DivAsCardContainer)dataInDb.JsxModel ).Items[0].TitleInfo.FreeTextValue.Should().Be("Aloha");

                database.DeleteByRequestName(data.RequestName).Should().Be(1);
            }
        }

        [TestMethod]
        public void SaveToFile()
        {
            //var items = BinarySerialization.Deserialize<List<ScreenInfo>>(File.ReadAllBytes("SaveToFile-3-29-11-33.bin"));

            //var visa = items.Where(x=>x.OutputTypeScriptFileName.IndexOf("visa",StringComparison.OrdinalIgnoreCase) >=0).ToList();


            using (var database = new DevelopmentDatabase())
            {
                var path = nameof(SaveToFile) + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + ".bin";
                File.WriteAllBytes(path, BinarySerialization.Serialize(database.GetAllScreens()));
            }
        }
        #endregion
    }
}