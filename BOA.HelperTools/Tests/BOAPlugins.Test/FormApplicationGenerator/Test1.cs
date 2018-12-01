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
            var template = RandomValue.Object<OrchestrationFileForListForm>();
            template.PushIndent("-------");
            template.PushIndent("___");

            var a = template.TransformText();
            template.PopIndent();

            a = template.TransformText();

            a.ToString();

        }
    }
}
