using BOA.DatabaseAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class BOACardDatabase : SqlDatabase
    {
        #region Constructors
        public BOACardDatabase() : base($"Server={@"srvxdev\zumrut"};Database={@"BOACard"};Trusted_Connection=True;")
        {
            CommandTimeout = 1000 * 60 * 60;
        }
        #endregion
    }
}