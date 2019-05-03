using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
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