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
@"01a

01234b";


            builder.AppendAll(Data);

            const string ExpectedResult=
@"      01a

      01234b";

            Assert.AreEqual(ExpectedResult, builder.ToString());
        }
    }
}