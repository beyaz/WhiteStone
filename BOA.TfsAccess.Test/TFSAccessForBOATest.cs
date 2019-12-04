using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.TfsAccess
{
    [TestClass]
    public class TFSAccessForBOATest
    {
        #region Properties
        static TFSAccessForBOA Api => new TFSAccessForBOA();
        #endregion

        #region Public Methods
        [TestMethod]
        [Ignore]
        public void CheckInFile()
        {
            // ARRANGE
            const string path = @"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOA.CodeGeneration\Common\SqlReaderMethods.cs";

            TFSAccessForBOA.CheckoutFile(path);

            // ACT  + ASSERT
            TFSAccessForBOA.CheckInFile(path, "INC292087# auto check in.");
        }

        [TestMethod]
        [Ignore]
        public void CheckInSolution()
        {
            var input = new CheckInSolutionInput
            {
                Comment          = "583# - Can Enter new Debit provizyon bilgisinin kaldırılması.",
                SolutionFilePath = @"D:\work\BOA.Kernel\Dev\BOA.Kernel.CardGeneral\BOA.Kernel.CardGeneral.sln"
            };

            TFSAccessForBOA.CheckInSolution(input);
        }

        [TestMethod]
        public void CheckOutFile()
        {
            // ARRANGE
            const string path = @"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOA.CodeGeneration\Common\SqlReaderMethods.cs";

            TFSAccessForBOA.CheckoutFile(path).Should().BeNull();
            TFSAccessForBOA.UndoCheckoutFile(path).Should().BeTrue();
        }

        [TestMethod]
        [Ignore]
        public void CreateWorkspace()
        {
            TFSAccessForBOA.CreateWorkspace(TFSAccessForBOA.KT, "BT3UG105NB2", "$/", @"D:\work");
        }

        [TestMethod]
        public void GetFileContent()
        {
            // ARRANGE
            var path = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Types.CardPaymentSystem.Clearing/FormAssistant.cs";

            // ACT
            var content = TFSAccessForBOA.GetFileContent(path);

            // ASSERT
            Assert.IsNotNull(content);
        }

        [TestMethod]
        public void GetSubFolderNames()
        {
            // ARRANGE
            const string tfsPathWithSearchPattern = @"$/BOA.BusinessModules/Dev/*";

            // ACT
            var content = TFSAccessForBOA.GetSubFolderNames(tfsPathWithSearchPattern);

            // ASSERT
            Assert.IsNotNull(content);
        }
        #endregion
    }
}