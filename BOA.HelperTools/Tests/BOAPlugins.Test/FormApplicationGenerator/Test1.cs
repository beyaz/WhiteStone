using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOA.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.FormApplicationGenerator
{
    [TestClass]
    public  class Test1
    {
        [TestMethod]
        public void A()
        {
            var a = new OrchestrationFile
            {
                GridColumnFields = new []{"A","B"},
                NamespaceNameForType = "f",
                ClassName = "u",
                DefinitionFormDataClassName = "yy",
                NamespaceName = "hh",
                RequestName = "Req"
            }.TransformText();
            a.ToString();

        }
    }
}
