using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class ProjectInjectorDebug
    {
        [TestMethod]
        public void CustomSqlInjection()
        {
            using (var kernel = new Kernel())
            {
                kernel.Get<ProjectInjector>().Inject("CC_OPERATIONS");
            }
        }
    }
}