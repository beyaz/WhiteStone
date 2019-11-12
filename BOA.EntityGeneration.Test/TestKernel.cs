using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    class TestKernel : Kernel
    {
        #region Constructors
        public TestKernel() : base(@"D:\github\WhiteStone\BOA.EntityGeneration.Test\BOA.EntityGeneration.json")
        {
        }
        #endregion
    }
}