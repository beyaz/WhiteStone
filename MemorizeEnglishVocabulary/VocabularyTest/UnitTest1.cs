﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfApp2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var words=new string[]{"Table","Card"};
            EnToTrCache.StartToInitializeCache(words);
        }
    }
}
