using BOA.CodeGeneration.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.TfsAccess.Test
{
    [TestClass]
    public class TFSAccessForBOATest
    {
        #region Public Methods
        // [TestMethod]
        public void CreateWorkspace()
        {
            TFSAccessForBOA.CreateWorkspace(TFSAccessForBOA.KT,"BT3UG105NB2","$/",@"D:\work");
        }
        #endregion
    }
}