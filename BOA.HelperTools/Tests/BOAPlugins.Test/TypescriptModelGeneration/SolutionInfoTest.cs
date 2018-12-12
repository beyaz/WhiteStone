using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.Utility.TypescriptModelGeneration
{
    [TestClass]
    public class SolutionInfoTest
    {
        #region Public Methods
        [TestMethod]
        public void Test()
        {
            const string SlnFilePath  = @"D:\Work\BOA.BusinessModules\Dev\BOA.CardGeneral.DebitCard\BOA.CardGeneral.DebitCard.sln";
            var          solutionInfo = SolutionInfo.CreateFrom(SlnFilePath);

            Assert.IsNotNull(solutionInfo.FilePathOf_FormAssistant_cs_In_Types);
        }
        #endregion
    }
}