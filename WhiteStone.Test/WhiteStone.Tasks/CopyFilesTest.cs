using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteStone.Tasks
{
    [TestClass]
    public class CopyFilesTest
    {
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