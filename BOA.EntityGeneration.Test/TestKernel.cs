using ___Company___.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    class TestKernel : Kernel
    {
        public static IDataContext CreateDataContext()
        {
            return Kernel.CreateDataContext(@"D:\github\WhiteStone\BOA.EntityGeneration.Test\BOA.EntityGeneration.json", false, null);
        }
    }
}