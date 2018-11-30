using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void DownloadDeepEnds()
        {

            DownloadHelper.EnsureNewtonsoftJson();
        }
    }
}
