﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteStone.Tasks
{
    [TestClass]
    public class CopyFilesTest
    {

        [TestMethod]
        public void MsBuildTest()
        {

            MSBuild.Build(new MSBuildData{  ProjectFilePath =@"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\BNS\BOA.Types.Kernel.Card.BNS\BOA.Types.Kernel.Card.BNS.csproj" });
        }


        #region Public Methods
        [TestMethod]
        public void GetFilePathsFromSource()
        {


            var filePaths = CopyFiles.ParseSource("d:\\temp\\A.txt   | yy.dll  | ").ToList();

            Assert.AreEqual("d:\\temp\\A.txt", filePaths[0]);
            Assert.AreEqual("d:\\temp\\yy.dll", filePaths[1]);
        }
        #endregion
    }


    
}