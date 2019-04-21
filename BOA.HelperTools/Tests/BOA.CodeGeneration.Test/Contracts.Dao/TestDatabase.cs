using BOA.DatabaseAccess;

namespace BOA.CodeGeneration.Contracts.Dao
{
    public class TestDatabase : SqlDatabase
    {
        public const string CatalogName = @"D:\GITHUB\WHITESTONE\BOA.HELPERTOOLS\TESTS\BOA.CODEGENERATION.TEST\DATABASE1.MDF";

        #region Constants
        const string ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\github\WhiteStone\BOA.HelperTools\Tests\BOA.CodeGeneration.Test\Database1.mdf; Integrated Security = True";
        #endregion

        #region Constructors
        public TestDatabase() : base(ConnectionString)
        {
        }
        #endregion
    }
}