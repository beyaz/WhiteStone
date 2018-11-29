using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class XmlHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void AddAttribute()
        {
            const string xmlString = @"<a><b></b></a>";

            var xDocument = XDocument.Parse(xmlString);

            XmlHelper.AddAttribute(xDocument.Root, "a/b", "x", "1");

            const string expected = @"<a><b x=""1""></b></a>";

            Assert.AreEqual(expected, XmlHelper.ClearXml(xDocument.ToString()));
        }
        [TestMethod]
        public void AddAttribute1()
        {
            const string xmlString = @"<a><b></b></a>";

            var xDocument = XDocument.Parse(xmlString);

            XmlHelper.AddAttribute(xDocument.Root, "a", "x", "1");

            const string expected = @"<a x=""1""><b></b></a>";

            Assert.AreEqual(expected, XmlHelper.ClearXml(xDocument.ToString()));
        }


        [TestMethod]
        public void AddAttribute2()
        {
            const string xmlString = @"<a><b></b></a>";

            var xDocument = XDocument.Parse(xmlString);

            const string expected = @"<a><b><c x=""1"" /></b></a>";
            const string expected2 = @"<a><b><c x=""2"" /></b></a>";

            // ACT
            XmlHelper.AddAttribute(xDocument.Root, "/a/b/c", "x", "1");

            // ASSERT
            Assert.AreEqual(expected, XmlHelper.ClearXml(xDocument.ToString()));


            // ACT
            XmlHelper.AddAttribute(xDocument.Root, "/a/b/c[x]=1", "x", "2");

            // ASSERT
            Assert.AreEqual(expected2, XmlHelper.ClearXml(xDocument.ToString()));
        }


        [TestMethod]
        public void AddAttribute3()
        {
            const string xmlString = @"<a><b></b></a>";

            var xDocument = XDocument.Parse(xmlString);

            const string expected  = @"<a><b><c z=""1"" x=""2"" /></b></a>";
            
            // ACT
            XmlHelper.AddAttribute(xDocument.Root, "/a/b/c[z]=1", "x", "2");

            // ASSERT
            Assert.AreEqual(expected, XmlHelper.ClearXml(xDocument.ToString()));
        }
        [TestMethod]
        public void AddAttribute4()
        {
            const string xmlString = @"<a><b z=""1""></b></a>";

            var xDocument = XDocument.Parse(xmlString);

            const string expected = @"<a><b z=""1""><c z=""2"" x=""3"" /></b></a>";

            // ACT
            XmlHelper.AddAttribute(xDocument.Root, "/a/b[z]=\"1\"/c[z]=2", "x", "3");

            // ASSERT
            Assert.AreEqual(expected, XmlHelper.ClearXml(xDocument.ToString()));
        }



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