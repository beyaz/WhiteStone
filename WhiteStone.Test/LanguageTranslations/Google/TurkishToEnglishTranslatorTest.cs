using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations.Google
{
    [TestClass]
    public class TurkishToEnglishTranslatorTest
    {
        #region Public Methods
        [TestMethod]
        public void TranslateEnglishToTurkish()
        {
            var english = TurkishToEnglishTranslator.Translate("araba");

            Assert.AreEqual("car", english);
        }
        #endregion
    }
}