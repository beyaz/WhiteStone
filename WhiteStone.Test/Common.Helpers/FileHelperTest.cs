using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class FileHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void GetZipFileFromGithub()
        {
            const string url = @"https://github.com/beyaz/WhiteStone/blob/master/BOA.OneDesigner/dist/BOA.OneDesigner.zip?raw=true";

            FileHelper.DownloadFile(url, @"d:\temp\BOA.OneDesigner.zip",true);
        }
        #endregion
    }
}