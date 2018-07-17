using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations.Longman
{
    [TestClass]
    public class LongmanTest
    {
        #region Public Methods
        [TestMethod]
        public void GetWordInfo()
        {
            var info = Translator.GetWordInfo("fly");

            Assert.AreEqual("Insects", info.Dictentries[0].Topics[2]);
        }
        #endregion
    }
}