using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Common;
using WhiteStone.Helpers;

namespace WhiteStone.Test.Helpers
{
    [Serializable]
    public class Model : ContractBase
    {
        public string A { get; set; }

        public List<string> StringList { get; set; }
    }

    [TestClass]
    public class ReflectionUtilityTest
    {
        [TestMethod]
        public void Clone()
        {
            var m = new Model {A = "Y", StringList = new List<string> {"T", "U"}};
            var m2 = m.Clone();
            Assert.AreEqual("Y", m2.A);
            Assert.AreEqual("U", m2.StringList[1]);
        }


        
    }
}