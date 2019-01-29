using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations.Longman
{
    [TestClass]
    public class TranslatorTest
    {
        #region Public Methods
        [TestMethod]
        public void Load()
        {
            var wordInfo = new WordInfo
            {
                Word                  = "fly",
                SkipInitializeTurkish = true
            };
            Translator.Load(wordInfo);

            wordInfo.Dictentries[0].Topics[2].Should().Be("Insects");
        }
        #endregion
    }
}