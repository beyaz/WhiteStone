using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    public class SerializationTestClassA
    {
        public SerializationTestClassA()
        {
            
        }
        public SerializationTestClassA(string stringProperty1)
        {
            StringProperty1 = stringProperty1;
        }
        #region Public Properties
        public string StringProperty1 { get; private set; }
        #endregion
    }

    public class SerializationTestClassB : SerializationTestClassA
    {
        #region Public Properties
        public new string StringProperty1 { get; set; }
        public     string StringProperty2 { get; private set; }
        #endregion
    }

    [TestClass]
    public class SerializationTest
    {
        #region Public Methods
        [TestMethod]
        public void Test()
        {
            var serializationTestClassA = new SerializationTestClassA("Prop1");
            var json = JsonHelper.Serialize(serializationTestClassA);
            Assert.AreEqual("",json);
        }
        #endregion
    }
}