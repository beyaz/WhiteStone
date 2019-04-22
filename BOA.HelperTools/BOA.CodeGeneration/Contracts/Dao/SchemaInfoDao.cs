using System.Collections.Generic;
using BOA.DatabaseAccess;

namespace BOA.CodeGeneration.Contracts.Dao
{
    public static class SchemaInfoDao
    {
        #region Public Methods
        public static IReadOnlyList<string> GetAllTableNamesInSchema(IDatabase database, string schema)
        {
            var sql = $@"

SELECT TABLE_NAME FROM information_schema.tables WHERE TABLE_TYPE='BASE TABLE' AND TABLE_SCHEMA = '{schema}'

";
            database.CommandText = sql;

            var tableNames = new List<string>();

            var reader = database.ExecuteReader();
            while (reader.Read())
            {
                tableNames.Add(reader["TABLE_NAME"].ToString());
            }

            reader.Close();

            return tableNames;
        }
        #endregion
    }
}