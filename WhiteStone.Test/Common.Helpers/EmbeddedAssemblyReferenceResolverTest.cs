using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class EmbeddedAssemblyReferenceResolverTest
    {
        [TestMethod]
        public void ShouldResolveEmbeddedAssemblyReference()
        {
            File.Delete(YamlHelper.ReferencedModuleName);
            YamlHelper.Serialize("string");
        }
    }
}