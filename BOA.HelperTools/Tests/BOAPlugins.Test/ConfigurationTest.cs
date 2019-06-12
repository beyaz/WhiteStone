using BOAPlugins.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BOAPlugins.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        #region Public Methods
        [TestMethod]
        public void DownloadDeepEnds()
        {
            var mock = new Mock<DownloadHelper>();
            mock.Setup(m => m.FileExists(It.IsAny<string>())).Returns(false);
            mock.Object.EnsureNewtonsoftJson();
        }
        #endregion
    }
}