using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class XmlHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void CleanTest()
        {
            var xml = @"<a> 
<b x=  'u '>jj   p</b>

</a>";

            xml = XmlHelper.ClearXml(xml);

            Assert.AreEqual(@"<a><b x=""u "">jj   p</b></a>", xml);
        }
        #endregion
    }
}