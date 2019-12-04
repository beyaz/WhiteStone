using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class EmbeddedAssemblyReferenceResolverTest
    {
        #region Constants
        internal const string ReferencedModuleName = "YamlDotNet.dll";
        #endregion

        #region Public Methods
        [TestMethod]
        public void ShouldResolveEmbeddedAssemblyReference()
        {
            File.Delete(ReferencedModuleName);

            YamlHelper.Serialize("string");
        }
        #endregion
    }
}