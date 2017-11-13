using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Common;

namespace WhiteStone.Test
{
    [TestClass]
    public class CastingTest
    {
        //public static T To<T>(object value)
        //{
        //    var converter = TypeDescriptor.GetConverter(typeof (T));

        //    return (T) converter.ConvertFrom(value);
        //}Assert.IsTrue(To<int?>(null) == null);


        [TestMethod]
        public void cast()
        {

            

            Assert.IsTrue(Cast.To<CastingTest>(this) == this);
            Assert.IsTrue(Cast.To<CastingTest>(this) == this);
        }
    }
}