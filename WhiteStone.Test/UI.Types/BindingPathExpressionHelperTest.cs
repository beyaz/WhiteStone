using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.UI.Types
{
    [TestClass]
    public class BindingPathExpressionHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void GetBindingPath()
        {
            var record = new MyClass();

            // ACT
            var bindingPath = BindingPathExpressionHelper.GetBindingPath(() => record.Inner.Int32Property0);

            Assert.AreEqual("Inner.Int32Property0", bindingPath);

            // ACT
            bindingPath = BindingPathExpressionHelper.GetBindingPath(() => record.Int32Property0);

            Assert.AreEqual("Int32Property0", bindingPath);
        }
        #endregion
    }
}