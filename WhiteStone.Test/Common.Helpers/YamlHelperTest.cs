using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class YamlHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void ShouldDeserializeIReadOnlyCollections()
        {
            var result = YamlHelper.Deserialize<DataClass>(@"
StringArray:
  - a
  - b
StringArray2:
  - a
  - c
");

            result.Should().BeEquivalentTo(new DataClass
            {
                StringArray = new[] {"a", "b"},
                StringArray2 = new List<string> {"a", "c"}
            });
        }
        #endregion

        class DataClass
        {
            #region Public Properties
            public string[] StringArray { get; set; }
            public IReadOnlyList<string> StringArray2 { get; set; }
            #endregion
        }
    }
}