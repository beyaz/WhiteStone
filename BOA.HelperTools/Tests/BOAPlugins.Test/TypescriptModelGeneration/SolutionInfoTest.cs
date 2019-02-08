using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.VSIntegration;
using FluentAssertions;
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

            var          solutionInfo = GenerateFilePathInfo.CreateFrom(SlnFilePath);

            Assert.IsNotNull(solutionInfo.FilePathOf_FormAssistant_cs_In_Types);

            FormAssistantProjectInitializer.Initialize(solutionInfo);
        }

        [TestMethod]
        public void Should_find_one_project_folder()
        {
            const string SlnFilePath = @"D:\work\BOA.CardModules\Dev\BOA.Card.Clearing.Visa\BOA.Card.Clearing.Visa.sln";

            var solutionInfo = GenerateFilePathInfo.CreateFrom(SlnFilePath);

            solutionInfo.SolutionInfo.OneProjectFolder.Should().Be(@"D:\work\BOA.CardModules\Dev\BOA.Card.Clearing.Visa\One\BOA.One.Card.ClearingVisa\");
        }
        #endregion
    }
}