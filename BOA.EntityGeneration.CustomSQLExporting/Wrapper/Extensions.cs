using System.Collections.Generic;
using BOA.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    public static class Extensions
    {
        #region Public Methods
        public static IReadOnlyList<string> GetProfileNames(this IDataContext context)
        {
            var profileIdList = new List<string>();
            var database      = context.Get(CustomSqlExporter.Database);
            var config        = context.Get(CustomSqlExporter.Config);

            database.CommandText = config.SQL_GetProfileIdList;
            var reader = database.ExecuteReader();
            while (reader.Read())
            {
                profileIdList.Add(reader["ProfileId"].ToString());
            }

            reader.Close();

            profileIdList.Add("*");

            return profileIdList;
        }
        #endregion
    }
}