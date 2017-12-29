using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.DatabaseAccess
{
    [TestClass]
    public class ExtensionsTest
    {
        #region Public Methods
        [TestMethod]
        public void SplitScript()
        {
            var arr = Extensions.SplitScript(@"

AA
GO
HH
AGOPT
").ToArray();

            Assert.AreEqual("AA", arr[0]);
            Assert.AreEqual(@"HH
AGOPT", arr[1]);
        }
        #endregion
    }
}