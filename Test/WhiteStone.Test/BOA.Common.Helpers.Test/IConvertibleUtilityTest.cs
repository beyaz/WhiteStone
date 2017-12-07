using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers.Test
{
    [TestClass]
    public class IConvertibleUtilityTest
    {
        #region Enums
        public enum MyEnum
        {
            A = 5,
            B,
            C
        }

        public enum MyEnum2 : byte
        {
            A = 5,
            B,
            C
        }
        #endregion

        #region Public Methods
        [TestMethod]
        public void To_Operation_Tests()
        {
            Assert.IsTrue("65".To<int?>() == 65);
            Assert.IsTrue("65".To<int>() == 65);
            Assert.IsTrue(DBNull.Value.To<int?>() == null);
            Assert.IsTrue(8.9.To<int?>() == 9);

            Assert.IsTrue("  ".To<int?>() == null);
            Assert.IsTrue("  ".To<string>() == "  ");
            Assert.IsTrue("              ".To<int?>() == null);
            Assert.IsTrue("  ".To<short?>() == null);
            Assert.IsTrue("".To<int?>() == null);
            Assert.IsTrue("".To<decimal?>() == null);
            Assert.IsTrue("".To<decimal>() == 0);
            Assert.IsTrue("6.8".To<decimal>() == 6.8M);

            var d = 6.8674326;
            Assert.IsTrue("6.8674326".To<double>().Equals(d));

            Assert.IsTrue("65 ".To<int?>() == 65);
            Assert.IsTrue(" 65 ".To<int?>() == 65);

            Assert.IsTrue(((short?) null).To<int?>() == null);
            Assert.IsTrue(((short?) null).To<int>() == 0);

            var a = MyEnum.A;
            Assert.AreEqual(a.To<int>(), 5);

            a = MyEnum.C;
            Assert.AreEqual(a.To<short>(), 7);

            var b = MyEnum2.A;
            Assert.AreEqual(b.To<int>(), 5);

            b = MyEnum2.C;
            Assert.AreEqual(b.To<short>(), 7);

            int? nullableInt = 6;
            Assert.AreEqual(nullableInt.To<int>(), 6);
            Assert.IsTrue(nullableInt.To<short?>() == 6);
            Assert.IsTrue(nullableInt.To<short>() == 6);
            Assert.AreEqual(nullableInt.To<string>(), "6");

            nullableInt = null;

            Assert.IsTrue(nullableInt.To<short?>() == nullableInt);

            Assert.IsTrue(nullableInt.To<string>() == null);
        }
        #endregion
    }
}