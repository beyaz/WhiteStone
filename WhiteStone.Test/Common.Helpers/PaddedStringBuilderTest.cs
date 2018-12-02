using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public  class PaddedStringBuilderTest
    {
        [TestMethod]
        public void AppendAll()
        {
            var builder = new PaddedStringBuilder
            {
                PaddingCount = 2,
                PaddingLength = 3
            };

            const string Data =
                @" a
  b";


            builder.AppendAll(Data);

            const string ExpectedResult=
                @"       a
        b";

            Assert.AreEqual(ExpectedResult, builder.ToString());
        }
    }
}