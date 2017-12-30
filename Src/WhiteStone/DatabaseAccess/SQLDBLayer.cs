﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BOA.DatabaseAccess
{
    public class SqlDatabaseLayer
    {
        #region Fields
        public Func<string, string> GetConnectionStringByConnectionName;
        #endregion

        #region Public Properties
        public bool StartTransaction { get; set; }

        public int Timeout { get; set; } = 200;
        #endregion

        #region Properties
        Dictionary<string, SqlConnection> Connections { get; } = new Dictionary<string, SqlConnection>();

        List<Tuple<SqlCommand, SqlDataReader>> DataReaders  { get; } = new List<Tuple<SqlCommand, SqlDataReader>>();
        Dictionary<string, SqlTransaction>     Transactions { get; } = new Dictionary<string, SqlTransaction>();
        #endregion

        #region Public Methods
        public void AddInParameter(SqlCommand sqlCommand, string name, SqlDbType dbType, object value)
        {
            AddInParameter(sqlCommand, name, dbType, value, 0);
        }

        public void AddInParameter(SqlCommand sqlCommand, string name, SqlDbType dbType, object value, int size)
        {
            SqlParameter param = new SqlParameter();
            ConfigureParameter(param, name, dbType, size, ParameterDirection.Input, false, 0, 0, string.Empty, value);
            sqlCommand.Parameters.Add(param);
        }

        public void BeginTransaction()
        {
            StartTransaction = true;
        }

        public void CommitTransaction()
        {
            foreach (var transaction in Transactions.Values)
            {
                transaction.Commit();
                transaction.Dispose();
            }

            StartTransaction = false;
        }

        public DataTable ExecuteDataTable(SqlCommand command)
        {
            DataTable dataTable  = new DataTable(Guid.NewGuid().ToString());
            var       dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);

            dataReader.Close();

            return dataTable;
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            var reader = command.ExecuteReader();

            DataReaders.Add(new Tuple<SqlCommand, SqlDataReader>(command, reader));

            return reader;
        }

        public T ExecuteScalar<T>(SqlCommand command)
        {
            var value = command.ExecuteScalar();

            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }

            return (T) value;
        }

        public SqlCommand GetDBCommand(string connectionName, string commandTextAsStoredProcedureName)
        {
            return CreateDBCommand(connectionName, commandTextAsStoredProcedureName, null, CommandType.StoredProcedure);
        }

        public void RollbackTransaction()
        {
            foreach (var transaction in Transactions.Values)
            {
                transaction.Rollback();
                transaction.Dispose();
            }

            StartTransaction = false;
        }
        #endregion

        #region Methods
        public void CloseConnections()
        {
            foreach (var connection in Connections.Values)
            {
                if (connection == null)
                {
                    continue;
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
          
        }

        static void ConfigureParameter(SqlParameter param, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, object value)
        {
            param.ParameterName = name;
            param.SqlDbType     = dbType;
            param.Size          = size;
            param.Value         = value ?? DBNull.Value;
            param.Direction     = direction;
            param.IsNullable    = nullable;
            param.SourceColumn  = sourceColumn;
            param.Precision     = precision;
            param.Scale         = scale;
        }

        SqlCommand CreateDBCommand(string key, string commandText, SqlParameter[] sqlParameters, CommandType commandType)
        {
            if (commandText == null)
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            var connection = GetConnection(key, 3);

            SqlTransaction transaction = null;
            Transactions.TryGetValue(key, out transaction);

            if (StartTransaction && transaction == null)
            {
                transaction = connection.BeginTransaction();

                Transactions[key] = transaction;
            }

            var dbCommand = new SqlCommand
            {
                CommandText    = commandText,
                CommandType    = commandType,
                Connection     = connection,
                Transaction    = transaction,
                CommandTimeout = Timeout
            };

            if (sqlParameters != null)
            {
                dbCommand.Parameters.AddRange(sqlParameters);
            }

            return dbCommand;
        }

        SqlConnection GetConnection(string key, int retryCount = 0)
        {
            SqlConnection connection = null;
            try
            {
                if (!Connections.TryGetValue(key, out connection))
                {
                    connection = GetDBConnection(key);

                    Connections[key] = connection;
                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return connection;
            }
            catch (Exception)
            {
                if (retryCount <= 0)
                {
                    throw;
                }

                return GetConnection(key, --retryCount);
            }
        }

        SqlConnection GetDBConnection(string key)
        {
            var sqlConnection = new SqlConnection(GetConnectionStringByConnectionName(key));
            sqlConnection.Open();
            return sqlConnection;
        }
        #endregion
    }
}