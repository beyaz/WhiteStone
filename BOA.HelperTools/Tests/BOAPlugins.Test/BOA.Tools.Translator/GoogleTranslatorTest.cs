using BOA.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace BOA.Tools.Translator
{
    [TestClass]
    public class GoogleTranslatorTest
    {
        [TestMethod]
        public void CreatePropertyNameFromSentence()
        {
            var result = GoogleTranslator.CreatePropertyNameFromSentence("Music lesson");

            Assert.AreEqual("MusicLesson", result);
        }

        void TranslateEnglishToGerman_TranslateExistingFile(string path)
        {
            var data = File.ReadAllText(path);

            var list = data.Split(Environment.NewLine.ToCharArray()).Where(x => x.IsNullOrWhiteSpace() == false).ToList();

            var convertAll = list.ConvertAll(GoogleTranslator.ReplaceEmbeddedEnglishStringsWithGermanVersions);

            var @join = string.Join(Environment.NewLine, convertAll);

            File.WriteAllText(path + ".German.txt", join);
        }
    }
}