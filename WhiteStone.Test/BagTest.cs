using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.ComponentModel
{
    [TestClass]
    public class BagTest
    {
        #region Constants
        const string A = "A";
        #endregion

        #region Public Methods
        [TestMethod]
        public void HasEntry()
        {
            var bag = new Bag();

            Assert.IsFalse(bag.ContainsKey(A));

            bag[A] = "Aloha";

            Assert.IsTrue(bag[A]?.ToString() == "Aloha");
        }

        [TestMethod]
        public void OperatorTest_for_int()
        {
            var bag = new Bag
            {
                [A] = 5
            };

            Assert.IsTrue(bag[A].IsSame(5));
            Assert.IsTrue(bag[A].ToInt32() != 6);

            Assert.IsTrue(bag[A].ToInt32() == 5);
            Assert.IsTrue(bag[A].ToInt32() != 6);

            Assert.IsTrue(bag[A].ToInt32() > 3);
        }

        [TestMethod]
        public void OperatorTest_for_string()
        {
            var bag = new Bag
            {
                [A] = "Aloha"
            };

            Assert.IsTrue(bag[A]?.ToString() == "Aloha");
            Assert.IsTrue(bag[A]?.ToString() != "Aloha2");
        }
        #endregion
    }
}