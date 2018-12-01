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
        public void TemplateDebug()
        {
            var template = RandomValue.Object<BCardSectionTemplate>();


            var a = template.TransformText();


            a.ToString();

        }
    }
}
