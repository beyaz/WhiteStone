﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Tasks
{
    [TestClass]
    public class SlnFileGeneratorTest
    {
        [TestMethod]
        public void Run()
        {
            SlnFileGenerator.GetBatFileContent(@"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes2\","BOA.Card.AutoGeneratedCodes");
        }
    }
}