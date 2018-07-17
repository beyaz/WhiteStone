using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations
{
    [TestClass]
    public class LongmanTest
    {
        #region Public Methods
        [TestMethod]
        public void GetWordInfo()
        {
            var info = Longman.GetWordInfo("fly");

            Assert.AreEqual("Insects", info.Dictentries[0].Topics[2]);
        }
        #endregion
    }
}