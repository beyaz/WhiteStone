using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers.Test
{
    public class TraceFileHelperTestContract
    {
        #region Public Properties
        public int A { get; set; }
        public string B { get; set; }
        #endregion
    }

    [TestClass]
    public class TraceFileHelperTest
    {
        #region Properties
        string Dir => Environment.GetFolderPath(Environment.SpecialFolder.Templates) + Path.DirectorySeparatorChar;
        string FilePath => Dir + "A.txt";
        #endregion

        #region Public Methods
        [TestMethod]
        public void Must_be_Create_If_File_Not_Exists()
        {
            File.Delete(FilePath);

            // ACT
            TraceFileHelper.PushToFile(FilePath, new TraceFileHelperTestContract {A = 488, B = "Aloha"});
            TraceFileHelper.PushToFile(FilePath, new TraceFileHelperTestContract { A = 488, B = "Aloha" });

            var contracts = TraceFileHelper.Read<TraceFileHelperTestContract>(FilePath).ToList();

            Assert.AreEqual(2, contracts.Count);
        }
        #endregion
    }
}