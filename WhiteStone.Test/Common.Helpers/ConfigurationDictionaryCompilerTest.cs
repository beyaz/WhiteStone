using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class ConfigurationDictionaryCompilerTest
    {
        [TestMethod]
        public void Compile()
        {

            var map = new Dictionary<string, string>
            {
                {"A", "B"},
                {"A2", "t$(A)"},
                {"A3", "9"},
                {"A4", "9$(A)-p-$(A2)"}
            };

            var compiledMap = ConfigurationDictionaryCompiler.Compile(map,(key,value)=>value);

            compiledMap.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                {"A", "B"},
                {"A2", "tB"},
                {"A3", "9"},
                {"A4", "9B-p-tB"}
            });

        }
    }
}
