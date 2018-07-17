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

            Assert.AreEqual("car", TurkishToEnglishTranslator.Translate("araba"));
            Assert.AreEqual("Kuşlar", EnglishToTurkishTranslator.Translate("Birds"));
        }
        #endregion
    }
}