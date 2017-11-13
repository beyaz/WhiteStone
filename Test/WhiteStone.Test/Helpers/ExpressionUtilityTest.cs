using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteStone.Helpers.Test
{
    /// <summary>
    ///     Defines the UI card general debit card common extensions test.
    /// </summary>
    [TestClass]
    public class ExpressionUtilityTest
    {
        #region Public Methods
        /// <summary>
        ///     Nameofs the method must return full property path.
        /// </summary>
        [TestMethod]
        public void NameofMethodMust_Return_Full_Property_Path()
        {
            var c = new TestClassForNameof();
            Assert.AreEqual("C3_Nullable", c.AccessPathOf(x => x.C3_Nullable));
            Assert.AreEqual("Inner.C3_Nullable", c.AccessPathOf(x => x.Inner.C3_Nullable));
            Assert.AreEqual("Inner.Inner.Inner.B3", c.AccessPathOf(xu => xu.Inner.Inner.Inner.B3));
        }
        #endregion

        /// <summary>
        ///     Defines the test class for nameof.
        /// </summary>
        class TestClassForNameof
        {
            #region Public Properties
            /// <summary>
            ///     Gets or sets the b3.
            /// </summary>
            public string B3 { get; set; }

            /// <summary>
            ///     Gets or sets the c3 nullable.
            /// </summary>
            public int? C3_Nullable { get; set; }

            /// <summary>
            ///     Gets or sets the inner.
            /// </summary>
            public TestClassForNameof Inner { get; set; }
            #endregion

            #region Public Methods
            public void ABV()
            {
            }
            #endregion
        }
    }
}