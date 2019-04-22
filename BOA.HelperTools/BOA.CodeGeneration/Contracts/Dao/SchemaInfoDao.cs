using System.Collections.Generic;
using BOA.DatabaseAccess;

namespace BOA.CodeGeneration.Contracts.Dao
{
    public static class SchemaInfoDao
    {
        public static IReadOnlyList<string> GetAllUserCreatedSchemaNames(IDatabase database)
        {
            var sql = $@"

SELECT name FROM sys.schemas WHERE principal_id = 1 and name<>'dbo'

";
            database.CommandText = sql;

            var tableNames = new List<string>();

            var reader = database.ExecuteReader();
            while (reader.Read())
            {
                tableNames.Add(reader["name"].ToString());
            }

            reader.Close();

            return tableNames;
        }


        
        #region Public Methods
        public static IReadOnlyList<string> GetAllTableNamesInSchema(IDatabase database, string schema)
        {
//            var sql = $@"

//SELECT TABLE_NAME FROM information_schema.tables WHERE TABLE_TYPE='BASE TABLE' AND TABLE_SCHEMA = '{schema}'

//";


            var sql = $@"

SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_SCHEMA = '{schema}'

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