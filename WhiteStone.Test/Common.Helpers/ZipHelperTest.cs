using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class ZipHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void CompressFolder()
        {
            var paths = new string[]
            {
                @"d:\temp\0.txt"
            };
            ZipHelper.CompressFiles(@"d:\temp\Aloha.zip", paths);

            ZipHelper.ExtractFromZipFile(@"d:\temp\Aloha.zip", null, new Dictionary<string, string> {{@"0.txt", @"d:\\temp\\01.txt"}});
        }
        #endregion
    }

    
}