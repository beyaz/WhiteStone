using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Helpers;

namespace BOA.LanguageTranslations
{
    [TestClass]
    public class GoogleTest
    {
        [TestMethod]
        public void TranslateEnglishToTurkish()
        {
            var wordInfo = Google.TranslateEnglishToTurkish("mean");

            Assert.AreEqual("anlamına gelmek", wordInfo.Explanation);
        }
    }
}