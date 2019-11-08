using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    class TestKernel : Kernel
    {
        #region Public Methods
        public override string GetConfigFilePath()
        {
            return @"D:\github\WhiteStone\BOA.EntityGeneration.Test\BOA.EntityGeneration.json";
        }
        #endregion
    }
}