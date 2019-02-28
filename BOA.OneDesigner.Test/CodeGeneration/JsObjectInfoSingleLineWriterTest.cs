using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class JsObjectInfoSingleLineWriterTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_write_each_field_in_one_line()
        {
            var data = new JsObject
            {
                {"a", "'a0'"},
                {"b", "true"}
            };

            JsObjectInfoSingleLineWriter.ToString(data).Should().Be("{ a: 'a0', b: true }");
        }
        #endregion
    }

    [TestClass]
    public class JsObjectInfoMultiLineWriterTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_write_each_field_in_multi_line()
        {
            var data = new JsObject
            {
                {"a", "'a0'"},
                {"b", "true"}
            };

            var expected =
                @"{
    a: 'a0',
    b: true
}";
            JsObjectInfoMultiLineWriter.ToString(data).Should().Be(expected);
        }
        #endregion
    }
}