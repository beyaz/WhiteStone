using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     The SQL database layer
    /// </summary>
    public class SqlDatabaseLayer
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the get connection by connection.
        /// </summary>
        public Func<string, DbConnection> GetConnectionByConnectionName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [start transaction].
        /// </summary>
        public bool StartTransaction { get; set; }

        /// <summary>
        ///     Gets or sets the timeout.
        /// </summary>
        public int Timeout { get; set; } = 200;
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the connections.
        /// </summary>
        Dictionary<string, DbConnection> Connections { get; } = new Dictionary<string, DbConnection>();

        /// <summary>
        ///     Gets the data readers.
        /// </summary>
        List<Tuple<DbCommand, IDataReader>> DataReaders { get; } = new List<Tuple<DbCommand, IDataReader>>();

        /// <summary>
        ///     Gets the transactions.
        /// </summary>
        Dictionary<string, DbTransaction> Transactions { get; } = new Dictionary<string, DbTransaction>();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the in parameter.
        /// </summary>
        public virtual void AddInParameter(DbCommand dbCommand, string name, SqlDbType dbType, object value)
        {
            var success = TryAddInParameterForMsSql(dbCommand, name, dbType, value);
            if (success)
            {
                return;
            }

            success = TryAddInParameterForMySql(dbCommand, name, dbType, value);
            if (success)
            {
                return;
            }

            throw new NotImplementedException(dbCommand.GetType().FullName);
        }

        /// <summary>
        ///     Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            StartTransaction = true;
        }

        /// <summary>
        ///     Closes the connections.
        /// </summary>
        public void CloseConnections()
        {
            foreach (var dataReader in DataReaders)
            {
                if (dataReader.Item2.IsClosed == false)
                {
                    throw new InvalidOperationException("DataReaders must be close.");
                }
            }

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

        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            foreach (var transaction in Transactions.Values)
            {
                transaction.Commit();
                transaction.Dispose();
            }

            Transactions.Clear();
            StartTransaction = false;
        }

        /// <summary>
        ///     Executes the data table.
        /// </summary>
        public DataTable ExecuteDataTable(DbCommand command)
        {
            var dataTable  = new DataTable(Guid.NewGuid().ToString());
            var dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);

            dataReader.Close();

            return dataTable;
        }

        /// <summary>
        ///     Executes the non query.
        /// </summary>
        public int ExecuteNonQuery(DbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Executes the reader.
        /// </summary>
        public DbDataReader ExecuteReader(DbCommand command)
        {
            var reader = command.ExecuteReader();

            DataReaders.Add(new Tuple<DbCommand, IDataReader>(command, reader));

            return reader;
        }

        /// <summary>
        ///     Executes the scalar.
        /// </summary>
        public T ExecuteScalar<T>(DbCommand command)
        {
            var value = command.ExecuteScalar();

            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }

            return (T) value;
        }

        /// <summary>
        ///     Gets the database command.
        /// </summary>
        public DbCommand GetDBCommand(Enum connectionName, string commandTextAsStoredProcedureName)
        {
            return GetDBCommand(connectionName.ToString(), commandTextAsStoredProcedureName);
        }

        /// <summary>
        ///     Gets the database command.
        /// </summary>
        public DbCommand GetDBCommand(string connectionName, string commandTextAsStoredProcedureName)
        {
            return CreateDBCommand(connectionName, commandTextAsStoredProcedureName, CommandType.StoredProcedure);
        }

        /// <summary>
        ///     Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            foreach (var transaction in Transactions.Values)
            {
                transaction.Rollback();
                transaction.Dispose();
            }

            Transactions.Clear();
            StartTransaction = false;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Tries the add in parameter for ms SQL.
        /// </summary>
        static bool TryAddInParameterForMsSql(DbCommand dbCommand, string name, SqlDbType dbType, object value)
        {
            var sqlCommand = dbCommand as SqlCommand;
            if (sqlCommand == null)
            {
                return false;
            }

            var dbParameter = new SqlParameter
            {
                ParameterName = name,
                SqlDbType     = dbType,
                Value         = value ?? DBNull.Value,
                Direction     = ParameterDirection.Input,
            };


            dbCommand.Parameters.Add(dbParameter);

            return true;
        }

        /// <summary>
        ///     Tries the add in parameter for my SQL.
        /// </summary>
        static bool TryAddInParameterForMySql(DbCommand dbCommand, string name, SqlDbType dbType, object value)
        {
            var mySqlCommand = dbCommand as MySqlCommand;
            if (mySqlCommand == null)
            {
                return false;
            }

            var mySqlDbType = MySqlDbType.VarChar;

            if (dbType == SqlDbType.Int)
            {
                mySqlDbType = MySqlDbType.Int32;
            }

            else if (dbType == SqlDbType.Decimal)
            {
                mySqlDbType = MySqlDbType.Decimal;
            }

            else if (dbType == SqlDbType.DateTime)
            {
                mySqlDbType = MySqlDbType.DateTime;
            }

            else if (dbType == SqlDbType.NVarChar || dbType == SqlDbType.VarChar)
            {
                mySqlDbType = MySqlDbType.VarChar;
            }

            else
            {
                throw new NotImplementedException(dbType.ToString());
            }

            var dbParameter = new MySqlParameter
            {
                ParameterName = name,
                MySqlDbType   = mySqlDbType,
                Size          = 0,
                Value         = value ?? DBNull.Value,
                Direction     = ParameterDirection.Input,
                IsNullable    = false,
                SourceColumn  = string.Empty,
                Precision     = 0,
                Scale         = 0
            };

            dbCommand.Parameters.Add(dbParameter);

            return true;
        }

        /// <summary>
        ///     Creates the database command.
        /// </summary>
        DbCommand CreateDBCommand(string key, string commandText, CommandType commandType)
        {
            if (commandText == null)
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            var connection = GetConnection(key, 3);

            var transaction = GetTransaction(key, connection);

            var command = TryCreateDbCommandForMsSql(connection, transaction, commandText, commandType);
            if (command != null)
            {
                return command;
            }

            command = TryCreateDbCommandForMySql(connection, transaction, commandText, commandType);
            if (command != null)
            {
                return command;
            }

            throw new NotImplementedException(connection.GetType().FullName);
        }

        /// <summary>
        ///     Gets the connection.
        /// </summary>
        DbConnection GetConnection(string key, int retryCount = 0)
        {
            DbConnection connection = null;
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

        /// <summary>
        ///     Gets the database connection.
        /// </summary>
        DbConnection GetDBConnection(string key)
        {
            var sqlConnection = GetConnectionByConnectionName(key);
            sqlConnection.Open();
            return sqlConnection;
        }

        /// <summary>
        ///     Gets the transaction.
        /// </summary>
        DbTransaction GetTransaction(string key, DbConnection connection)
        {
            DbTransaction transaction = null;

            Transactions.TryGetValue(key, out transaction);

            if (StartTransaction && transaction == null)
            {
                transaction = connection.BeginTransaction();

                Transactions[key] = transaction;
            }

            return transaction;
        }

        /// <summary>
        ///     Tries the create database command for ms SQL.
        /// </summary>
        DbCommand TryCreateDbCommandForMsSql(DbConnection connection, DbTransaction transaction, string commandText, CommandType commandType)
        {
            var sqlConnection = connection as SqlConnection;

            if (sqlConnection == null)
            {
                return null;
            }

            return new SqlCommand
            {
                CommandText    = commandText,
                CommandType    = commandType,
                Connection     = sqlConnection,
                Transaction    = (SqlTransaction) transaction,
                CommandTimeout = Timeout
            };
        }

        /// <summary>
        ///     Tries the create database command for my SQL.
        /// </summary>
        DbCommand TryCreateDbCommandForMySql(DbConnection connection, DbTransaction transaction, string commandText, CommandType commandType)
        {
            var mySqlConnection = connection as MySqlConnection;
            if (mySqlConnection == null)
            {
                return null;
            }

            return new MySqlCommand
            {
                CommandText    = commandText,
                CommandType    = commandType,
                Connection     = mySqlConnection,
                Transaction    = (MySqlTransaction) transaction,
                CommandTimeout = Timeout
            };
        }
        #endregion
    }
}