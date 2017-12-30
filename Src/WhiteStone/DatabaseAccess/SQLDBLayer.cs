using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     The SQL database layer
    /// </summary>
    public class SqlDatabaseLayer
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the get connection string by connection.
        /// </summary>
        public Func<string, string> GetConnectionStringByConnectionName { get; set; }

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
        Dictionary<string, SqlConnection> Connections { get; } = new Dictionary<string, SqlConnection>();

        /// <summary>
        ///     Gets the data readers.
        /// </summary>
        List<Tuple<DbCommand, IDataReader>> DataReaders { get; } = new List<Tuple<DbCommand, IDataReader>>();

        /// <summary>
        ///     Gets the transactions.
        /// </summary>
        Dictionary<string, SqlTransaction> Transactions { get; } = new Dictionary<string, SqlTransaction>();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the in parameter.
        /// </summary>
        public void AddInParameter(SqlCommand sqlCommand, string name, SqlDbType dbType, object value)
        {
            AddInParameter(sqlCommand, name, dbType, value, 0);
        }

        /// <summary>
        ///     Adds the in parameter.
        /// </summary>
        public void AddInParameter(SqlCommand sqlCommand, string name, SqlDbType dbType, object value, int size)
        {
            var param = new SqlParameter();
            ConfigureParameter(param, name, dbType, size, ParameterDirection.Input, false, 0, 0, string.Empty, value);
            sqlCommand.Parameters.Add(param);
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
        public SqlDataReader ExecuteReader(SqlCommand command)
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
        public SqlCommand GetDBCommand(Enum connectionName, string commandTextAsStoredProcedureName)
        {
            return GetDBCommand(connectionName.ToString(), commandTextAsStoredProcedureName);
        }

        /// <summary>
        ///     Gets the database command.
        /// </summary>
        public SqlCommand GetDBCommand(string connectionName, string commandTextAsStoredProcedureName)
        {
            return CreateDBCommand(connectionName, commandTextAsStoredProcedureName, null, CommandType.StoredProcedure);
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
        ///     Configures the parameter.
        /// </summary>
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

        /// <summary>
        ///     Creates the database command.
        /// </summary>
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

        /// <summary>
        ///     Gets the connection.
        /// </summary>
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

        /// <summary>
        ///     Gets the database connection.
        /// </summary>
        SqlConnection GetDBConnection(string key)
        {
            var sqlConnection = new SqlConnection(GetConnectionStringByConnectionName(key));
            sqlConnection.Open();
            return sqlConnection;
        }
        #endregion
    }
}