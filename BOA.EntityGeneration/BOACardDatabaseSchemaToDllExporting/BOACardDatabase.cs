using BOA.DatabaseAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class BOACardDatabase : SqlDatabase
    {
        #region Constructors
        public BOACardDatabase() : base($"Server={@"srvxdev\zumrut"};Database={@"BOACard"};Trusted_Connection=True;")
        {
        }
        #endregion
    }
}