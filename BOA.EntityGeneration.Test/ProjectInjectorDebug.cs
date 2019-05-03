using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    [TestClass]
    public class ProjectInjectorDebug
    {
        [TestMethod]
        public void CustomSqlInjection()
        {
            using (var kernel = new Kernel())
            {
                 
                 // bize ait değil kernel.Get<ProjectInjector>().Inject("CRD_MANAGEMENT");
                 kernel.Get<ProjectInjector>().Inject("TMS");

                // // kernel.Get<ProjectInjector>().Inject("ACQUIRING");
                // // kernel.Get<ProjectInjector>().Inject("ACQUIRING_APPLICATION");
                // // kernel.Get<ProjectInjector>().Inject("BKM_MANAGEMENT");
                // // kernel.Get<ProjectInjector>().Inject("CARD_APPLICATION");
                // // kernel.Get<ProjectInjector>().Inject("CC_OPERATIONS");
                // // kernel.Get<ProjectInjector>().Inject("CLEARING_COMMON");
                // // kernel.Get<ProjectInjector>().Inject("CRD_INSTANT");
                // // kernel.Get<ProjectInjector>().Inject("CRD_ISSUING");
                // // kernel.Get<ProjectInjector>().Inject("EMC_MANAGEMENT");
                // // kernel.Get<ProjectInjector>().Inject("PM_MANAGEMENT");
                // // kernel.Get<ProjectInjector>().Inject("SWITCH_BKM");
                // // kernel.Get<ProjectInjector>().Inject("SYS_OPERATION");
                // // kernel.Get<ProjectInjector>().Inject("SYSTEM_MANAGEMENT");
                // // kernel.Get<ProjectInjector>().Inject("VISA_MANAGEMENT");

            }
        }
    }
}