using System.Collections.Generic;
using System.IO;
using FluentAssertions;
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
            const string simpleFilePath = @"d:\temp\0.txt";

            File.Delete(simpleFilePath);
            File.WriteAllText(simpleFilePath,"Test");

            var paths = new[]
            {
                simpleFilePath
            };
            const string zipFilePath = @"d:\temp\Aloha.zip";

            ZipHelper.CompressFiles(zipFilePath, paths);

            ZipHelper.HasEntry(zipFilePath, "0.txt").Should().BeTrue();
            ZipHelper.HasEntry(zipFilePath, "0.txt").Should().BeTrue();
            ZipHelper.HasEntry(zipFilePath, "0.txt").Should().BeTrue();

            ZipHelper.HasEntry(zipFilePath, "1.txt").Should().BeFalse();

            ZipHelper.ExtractFromZipFile(zipFilePath, null, new Dictionary<string, string> {{@"0.txt", @"d:\\temp\\01.txt"}});
        }
        #endregion
    }
}