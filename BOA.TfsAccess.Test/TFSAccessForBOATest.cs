using BOA.CodeGeneration.Util;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.TfsAccess.Test
{
    [TestClass]
    public class TFSAccessForBOATest
    {
        #region Public Methods
        [TestMethod]
        public void CheckInSolution()
        {
            var data = new CheckInSolutionInput
            {
                SolutionFilePath = @"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\BOA.Kernel.Card.sln",
                Comment          = "2235# - fix"
            };

            TFSAccessForBOA.CheckInSolution(data);

            data.ResultMessage.Should().BeNull();
        }
        #endregion
    }
}