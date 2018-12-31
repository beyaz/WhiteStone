using BOAPlugins.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        #region Public Methods
        [TestMethod]
        public void DownloadDeepEnds()
        {
            DownloadHelper.EnsureNewtonsoftJson();
        }
        #endregion
    }
}