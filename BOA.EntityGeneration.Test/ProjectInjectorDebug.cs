using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    [TestClass]
    public class ProjectInjectorDebug
    {
        #region Public Methods
        [TestMethod]
        public void CustomSqlInjection()
        {
            var context = Kernel.CreateDataContext(null, false, null);
            ProjectInjector.Inject(context, "CC_OPERATIONS");
        }
        #endregion
    }
}