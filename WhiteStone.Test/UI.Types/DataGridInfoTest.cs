using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.UI.Types
{
    [TestClass]
    public class DataGridInfoTest

    {
        #region Public Methods
        [TestMethod]
        public void AddColumn()
        {
            var record = new MyClass();

            var dataGridInfo = DataGridInfo.Create(record.GetType());

            dataGridInfo.SetMessaging(typeof(BindingPathExpressionHelperTest), propertyName => "aloha");

            dataGridInfo.AddColumn(() => record.Inner.Int32Property0);
            dataGridInfo.AddColumn(() => record.Inner);

            dataGridInfo.AddRowBackground(()=>record.StringProperty0,string.Empty);

            Assert.AreEqual("inner.int32Property0", dataGridInfo.Columns[0].BindingPath);
            Assert.AreEqual("aloha", dataGridInfo.Columns[0].Label);

            Assert.AreEqual("inner", dataGridInfo.Columns[1].BindingPath);
            Assert.AreEqual("aloha", dataGridInfo.Columns[1].Label);

            Assert.AreEqual("stringProperty0", dataGridInfo.RowBackgrounds[0].BindingPath);
            Assert.AreEqual(string.Empty, dataGridInfo.RowBackgrounds[0].Color);
        }
        #endregion
    }
}