using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteStone.Tasks
{
    [TestClass]
    public class SemanticUIIconNamesEvaluaterTest
    {
        #region Public Methods
        [TestMethod]
        public void Run()
        {
            var api = new SemanticUIIconNamesEvaluater();

            api.Run();
        }
        #endregion
    }
}