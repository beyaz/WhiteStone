using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionsAreEvil.Classifications;

namespace UnitTestProject1
{
    [TestClass]
    public class OptionsTest
    {
        #region Public Methods
        [TestMethod]
        public void ShouldReadOptionFromFile()
        {
            Assert.IsTrue(Options.IsReadFromFile);
        }
        #endregion
    }
}