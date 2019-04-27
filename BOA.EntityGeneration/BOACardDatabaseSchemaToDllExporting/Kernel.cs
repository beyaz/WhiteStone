using BOA.DatabaseAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class Kernel : StandardKernel
    {
        #region Constructors
        public Kernel()
        {
            Bind<IDatabase>().To<BOACardDatabase>().InSingletonScope();
            Bind<ScriptModel.Creators.InsertInfoCreator>().To<InsertInfoCreator>();
        }
        #endregion
    }
}