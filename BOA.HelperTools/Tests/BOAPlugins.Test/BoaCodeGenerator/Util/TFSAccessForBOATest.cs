using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Util
{
    [TestClass]
    public class TFSAccessForBOATest
    {
        #region Properties
        static TFSAccessForBOA Api => new TFSAccessForBOA();
        #endregion

        #region Public Methods
        [TestMethod]
        public void CheckInFile()
        {
            // ARRANGE
            var path = @"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOA.CodeGeneration\Common\SqlReaderMethods.cs";

            TFSAccessForBOA.CheckoutFile(path);

            // ACT  + ASSERT
            //TFSAccessForBOA.CheckInFile(path, "INC292087# auto check in.");
        }

        [TestMethod]
        public void CheckInSolution()
        {
            var input = new CheckInSolutionInput
            {
                Comment          = "583# - Can Enter new Debit provizyon bilgisinin kaldırılması.",
                SolutionFilePath = @"D:\work\BOA.Kernel\Dev\BOA.Kernel.CardGeneral\BOA.Kernel.CardGeneral.sln"
            };

            // TFSAccessForBOA.CheckInSolution(input);
        }

        [TestMethod]
        public void CheckOutFile()
        {
            // ARRANGE
            var path = @"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOA.CodeGeneration\Common\SqlReaderMethods.cs";
            // path = @"D:\workde\BOA.BusinessModules\Dev\BOA.Card.CreditCardOperation\BOA.Business.Card.CreditCardOperation\BOA.Business.Kernel.CreditCard\CreditCardInstallmentTransaction.designer.cs";

            TFSAccessForBOA.CheckoutFile(path).Should().BeTrue();
            TFSAccessForBOA.UndoCheckoutFile(path).Should().BeTrue();
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