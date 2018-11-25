using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Helpers;

namespace WhiteStone.Test
{
    [TestClass]
    public class LıstUtilityTests
    {
        #region Public Methods
        [TestMethod]
        public void Split()
        {
            var list   = new List<string> {"0", "1", "2"};
            var result = list.Split(1);
            Assert.IsTrue(result.Length == 3);

            result = list.Split(2);
            Assert.IsTrue(result.Length == 2);

            Assert.IsTrue(result[0][0] == "0");
            Assert.IsTrue(result[0][1] == "1");

            Assert.IsTrue(result[1][0] == "2");
        }

        [TestMethod]
        public void SplitArray()
        {
            var list   = new[] {"0", "1", "2"};
            var result = list.Split(1);
            Assert.IsTrue(result.Length == 3);

            result = list.Split(2);
            Assert.IsTrue(result.Length == 2);

            Assert.IsTrue(result[0][0] == "0");
            Assert.IsTrue(result[0][1] == "1");

            Assert.IsTrue(result[1][0] == "2");
        }
        #endregion
    }
}