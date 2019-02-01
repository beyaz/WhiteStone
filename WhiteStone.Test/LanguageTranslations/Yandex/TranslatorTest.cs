using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations.Yandex
{
    [TestClass]
    public class TranslatorTest
    {
        #region Public Methods
        [TestMethod]
        public void Translate()
        {
            Translator.TranslateTurkishToEnglish("araba").Should().Be("car");
            Translator.TranslateEnglishToTurkish("Geography").Should().Be("Coğrafya");
            Translator.TranslateEnglishToTurkish("Birds").Should().Be("Kuşlar");
        }
        #endregion
    }
}