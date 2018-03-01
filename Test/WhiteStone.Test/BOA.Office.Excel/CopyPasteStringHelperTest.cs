using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BOA.Office.Excel
{
    [TestClass]
    public class CopyPasteStringHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void CopyPasteTest()
        {
            var items = CreateTestContracts();

            var propertyNames = new[]
            {
                nameof(TestClass.DecimalProp0),
                nameof(TestClass.DecimalProp1),

                nameof(TestClass.StringProp1),
                nameof(TestClass.StringProp0),

                nameof(TestClass.Int32Prop0),
                nameof(TestClass.Int32Prop1)
            };

            var result = CopyPasteStringHelper.PrepareForPasteToExcel(items, propertyNames);

            const string expected = @"56.78	43.02	ye2öçştj	Test6klö	5	6
56.71	43.03	ye2öçştj_1	Test6klö_1	57	63
56.72	43.03	ye2öçştj_2_____________ğ__________ü	Test6klö_2	58	65";

            Assert.AreEqual(expected, result);

            var deserializedItems = CopyPasteStringHelper.ParseFromString<TestClass>(expected, propertyNames);

            Assert.AreEqual(JsonConvert.SerializeObject(items), JsonConvert.SerializeObject(deserializedItems));
        }
        #endregion

        #region Methods
        static TestClass[] CreateTestContracts()
        {
            return new[]
            {
                new TestClass
                {
                    DecimalProp0 = 56.78M,
                    DecimalProp1 = 43.02M,
                    Int32Prop0   = 5,
                    Int32Prop1   = 6,
                    StringProp0  = "Test6klö",
                    StringProp1  = "ye2öçştj"
                },
                new TestClass
                {
                    DecimalProp0 = 56.71M,
                    DecimalProp1 = 43.03M,
                    Int32Prop0   = 57,
                    Int32Prop1   = 63,
                    StringProp0  = "Test6klö_1",
                    StringProp1  = "ye2öçştj_1"
                },
                new TestClass
                {
                    DecimalProp0 = 56.72M,
                    DecimalProp1 = 43.03M,
                    Int32Prop0   = 58,
                    Int32Prop1   = 65,
                    StringProp0  = "Test6klö_2",
                    StringProp1  = "ye2öçştj_2_____________ğ__________ü"
                }
            };
        }
        #endregion

        class TestClass
        {
            #region Public Properties
            public decimal? DecimalProp0 { get; set; }
            public decimal? DecimalProp1 { get; set; }

            public int    Int32Prop0  { get; set; }
            public int    Int32Prop1  { get; set; }
            public string StringProp0 { get; set; }
            public string StringProp1 { get; set; }
            #endregion
        }
    }
}