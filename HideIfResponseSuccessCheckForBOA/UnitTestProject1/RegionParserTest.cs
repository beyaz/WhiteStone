using System;
using System.IO;
using JavaScriptRegions;
using BOAPlugins.HideSuccessCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class RegionParserTest
    {

        [TestMethod]
        public void SimpleCallWithKnownMethodWithNullConditionalOperator()
        {
            const string sourceText = @"
            
            var checkResponse =   boCardStatusChecker.CheckCardStatus(data);
            if (!checkResponse.Success)
            {
                returnObject.Results.AddRange(checkResponse.Results);
                return returnObject;
            }
            var x = checkResponse.Value?.FirstOrDefault();

";
            var data = new RegionParserTestData
            {
                SourceText = sourceText
            };

            RegionParser.Parse(data);

            Assert.AreEqual("var x = boCardStatusChecker.CheckCardStatus(data)?.FirstOrDefault();", data.Regions?[0]?.Text);
        }

        [TestMethod]
        public void SimpleCallWithKnownMethod()
        {
            const string sourceText = @"
            
            var checkResponse =   boCardStatusChecker.CheckCardStatus(data);
            if (!checkResponse.Success)
            {
                returnObject.Results.AddRange(checkResponse.Results);
                return returnObject;
            }
            var x = checkResponse.Value.GetValueOrDefault();

";
            var data = new RegionParserTestData
            {
                SourceText = sourceText
            };

            RegionParser.Parse(data);

            Assert.AreEqual("var x = boCardStatusChecker.CheckCardStatus(data).GetValueOrDefault();", data.Regions?[0]?.Text);
        }

        [TestMethod]
        public void SimpleCallWithEqualityComparison()
        {
            const string sourceText = @"
            
            var checkResponse =   boCardStatusChecker.CheckCardStatus(data);
            if (!checkResponse.Success)
            {
                returnObject.Results.AddRange(checkResponse.Results);
                return returnObject;
            }

            var x = checkResponse.Value != null;

";
            var data = new RegionParserTestData
            {
                SourceText = sourceText
            };

            RegionParser.Parse(data);

            Assert.AreEqual("var x = boCardStatusChecker.CheckCardStatus(data) != null;", data.Regions?[0]?.Text);
        }


        #region Public Methods
        [TestMethod]
        public void LongCall()
        {
            const string sourceText = @"
            
            var checkResponse = new CardStatusChecker {Context = objectHelper.Context}.CheckCardStatus(data);
            if (!checkResponse.Success)
            {
            
                // test
                
                returnObject.Results.AddRange(checkResponse.Results);
                // any comment
                return returnObject;
            }

";
            var data = new RegionParserTestData
            {
                SourceText = sourceText
            };

            RegionParser.Parse(data);

            Assert.AreEqual("new CardStatusChecker {Context = objectHelper.Context}.CheckCardStatus(data);", data.Regions?[0]?.Text);
        }

        [TestMethod]
        public void SimpleCall()
        {
            const string sourceText = @"
            
            var checkResponse =   boCardStatusChecker.CheckCardStatus(data);
            if (!checkResponse.Success)
            {
                returnObject.Results.AddRange(checkResponse.Results);
                return returnObject;
            }

";
            var data = new RegionParserTestData
            {
                SourceText = sourceText
            };

            RegionParser.Parse(data);

            Assert.AreEqual("boCardStatusChecker.CheckCardStatus(data);", data.Regions?[0]?.Text);
        }

        [TestMethod]
        public void TestFile()
        {
            const string FilePath = @"D:\Work\BOA.Kernel\Dev\BOA.Kernel.CardGeneral\DebitCard\BOA.Engine.DebitCard\Utility\Validation.cs";

            var data = new RegionParserTestData
            {
                SourceText = File.ReadAllText(FilePath)
            };

            RegionParser.Parse(data);

            Assert.IsTrue(data.Regions.Count > 0);
        }

        [TestMethod]
        public void TestLargeFile()
        {
            const string FilePath = @"D:\Work\BOA.BusinessModules\Dev\BOA.CardGeneral.DebitCard\BOA.Orchestration.CardGeneral.DebitCard\CardTransactionListForm.cs";

            var data = new RegionParserTestData
            {
                SourceText = File.ReadAllText(FilePath)
            };

            RegionParser.Parse(data);

            Assert.IsTrue(data.Regions.Count > 0);

            Log.Push(new ArgumentException("xxx"));
        }
        #endregion
    }
}