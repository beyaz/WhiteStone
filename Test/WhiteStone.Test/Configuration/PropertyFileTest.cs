using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Configuration;

namespace WhiteStone.Test.Configuration
{
    [TestClass]
    public class PropertyFileTest
    {
        [TestMethod]
        public void Read()
        {
            var file = new PropertyFile().LoadFromFile("Configuration\\PropertyFileTestData.property");

            Assert.AreEqual("http\\://en.wikipedia.org/", file["website"]);

            Assert.AreEqual(@"Welcome to 
          Wikipedia!", file["message"]);

            Assert.AreEqual("This is the value that could be looked up with the key \"key with spaces\".", file["key with spaces"]);
            Assert.AreEqual("Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", file["DbConnectionString"]);
        }
    }
}