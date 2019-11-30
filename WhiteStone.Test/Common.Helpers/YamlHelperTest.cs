using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class YamlHelperTest
    {
        class DataClass
        {
            public string[] StringArray { get; set; }

        }
        [TestMethod]
        public void A()
        {
     var result =       YamlHelper.Deserialize<DataClass>(@"
StringArray:
  - a
  - b
");

            result.Should().

        }
    }
}
