using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class ZipHelperTest
    {
        [TestMethod]
        public void CompressFolder()
        {
            ZipHelper.CompressFiles(@"d:\temp\Aloha.zip",null,@"d:\temp\0.txt");
        }
    }
}
