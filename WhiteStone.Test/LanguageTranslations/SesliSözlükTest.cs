using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations
{
    [TestClass]
    public class SesliSözlükTest
    {
        #region Public Methods
        [TestMethod]
        public void TranslateEnglishToTurkish()
        {
            var wordInfo = SesliSözlük.GetWord("mean");

            Assert.AreEqual("min", wordInfo.TurkishPronanciation);

            Assert.AreEqual(220, wordInfo.Means.Count);

            Assert.AreEqual(2, wordInfo.Means[3].SampleSentences.Count);

            Assert.AreEqual("orta", wordInfo.Means[3].Definition);
        }
        #endregion
    }
}