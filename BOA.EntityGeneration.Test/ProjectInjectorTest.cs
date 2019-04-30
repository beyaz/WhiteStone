using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{

    class ProjectInjectorTestKernel : Kernel
    {
    }

    [TestClass]
    public class ProjectInjectorTest
    {
        [TestMethod]
        public void CustomSqlInjection()
        {
            using (var kernel = new ProjectInjectorTestKernel())
            {
                kernel.Get<ProjectInjector>().Inject("CC_OPERATIONS");
            }
        }
    }
}