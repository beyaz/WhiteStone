using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Util
{
    [TestClass]
    public class TFSUndoHelperTest
    {
        static TFSAccessForBOA Api => new TFSAccessForBOA();

        [TestMethod]
        public void CheckinFile()
        {
            // ARRANGE
            var path = @"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOA.CodeGeneration\Common\SqlReaderMethods.cs";

            TFSAccessForBOA.CheckoutFile(path);

            // ACT  + ASSERT
            TFSAccessForBOA.CheckInFile(path, "INC292087# auto check in.");
        }
    }
}