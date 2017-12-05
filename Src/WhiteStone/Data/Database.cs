using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using BOA.Data.Attributes;

namespace BOA.Data
{
    /// <summary>
    ///     Manager of sql operations
    /// </summary>
    public abstract class Database : IDatabase
    {
        /// <summary>
        ///     Adds dataparameter to Command
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object this[string parameterName]
        {
            set { _command.Parameters.Add(CreateParameter(parameterName, value)); }
            get
            {
                if (_command == null)
                {
                    return null;
                }

                return (from IDbDataParameter parameter in _command.Parameters
                    where parameter.ParameterName == parameterName
                    select parameter.Value).FirstOrDefault();
            }
        }

        #region Fields
        IDbConnection _connection;
        IDbTransaction _transaction;
        IDbCommand _command;
        #endregion

        #region Constructor
        /// <summary>
        ///     Manager of sql operations
        /// </summary>
        protected Database(string connectionString)
            : this(new SqlConnection(connectionString))
        {
        }

        /// <summary>
        ///     Manager of sql operations
        /// </summary>
        protected Database(IDbConnection connection)
        {
            OpenConnection(connection);
        }
        #endregion

        #region Properties
        /// <summary>
        ///     The time (in seconds) to wait for the command to execute. The default value 30 seconds in .net
        ///     <para>  value is null  </para>
        /// </summary>
        public int? CommandTimeout { set; get; }

        /// <summary>
        ///     Gets prefix of sql parameters.
        /// </summary>
        public abstract string ParameterPrefix { get; }

        #region string CommandText
        /// <summary>
        ///     Gets or sets the text command to run against the data source.
        /// </summary>
        public string CommandText
        {
            set
            {
                _command = CreateCommand();
                _command.CommandText = value;
            }
            get
            {
                if (_command == null)
                {
                    return null;
                }
                return _command.CommandText;
            }
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        ///     Starts database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        ///     Commits database transaction.
        /// </summary>
        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        /// <summary>
        ///     Rollback database transaction.
        /// </summary>
        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        ///     if has any transaction then rollback transaction.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     if has any transaction then rollback transaction.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    Rollback();
                }
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }

        void OpenConnection(IDbConnection connection)
        {
            _connection = connection;
            if (connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        IDbCommand CreateCommand()
        {
            var c = _connection.CreateCommand();
            if (CommandTimeout != null)
            {
                c.CommandTimeout = (int) CommandTimeout;
            }
            if (_transaction != null)
            {
                c.Transaction = _transaction;
            }
            c.CommandType = CommandType.Text; // default
            return c;
        }

        #region Execution
        /// <summary>
        ///     Executes the System.Data.IDbCommand.CommandText against the System.Data.IDbCommand.Connection
        ///     and builds an System.Data.IDataReader.
        /// </summary>
        /// <returns></returns>
        public IDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }

        /// <summary>
        ///     Executes the query, and returns the first column of the first row in the resultset
        ///     returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }

        /// <summary>
        ///     Executes an SQL statement against the Connection object of a .NET Framework data
        ///     provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Indicates CommandType is StoredProcedure or Text
        /// </summary>
        /// <returns></returns>
        public bool CommandIsStoredProcedure
        {
            set
            {
                if (value)
                {
                    _command.CommandType = CommandType.StoredProcedure;
                    return;
                }

                _command.CommandType = CommandType.Text;
            }
            get { return _command.CommandType == CommandType.StoredProcedure; }
        }
        #endregion

        /// <summary>
        ///     Initializes new <see cref="IDbDataParameter" /> instance.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract IDbDataParameter CreateParameter(string parameterName, object value);
        #endregion

        SqlGenerator GetGenerator()
        {
            return new SqlGenerator
            {
                ParameterPrefix = ParameterPrefix,
                CreateDbDataParameter = CreateParameter
            };
        }

        #region DML
        /// <summary>
        ///     Inserts the specified entity contract.
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        public void Insert(object entityContract)
        {
            var result = GetGenerator().GenerateInsertStatementFromEntityContract(entityContract);

            foreach (var parameter in result.GeneratedParameters)
            {
                _command.Parameters.Add(parameter);
            }

            ExecuteNonQuery();
        }

        /// <summary>
        ///     Deletes the specified entity contract.
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        public void Delete(object entityContract)
        {
            var result = GetGenerator().GenerateDeleteStatementFromEntityContract(entityContract);

            foreach (var parameter in result.GeneratedParameters)
            {
                _command.Parameters.Add(parameter);
            }

            ExecuteNonQuery();
        }

        /// <summary>
        ///     Updates the specified entity contract.
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        public void Update(object entityContract)
        {
            var result = GetGenerator().GenerateUpdateStatementFromEntityContract(entityContract);

            foreach (var parameter in result.GeneratedParameters)
            {
                _command.Parameters.Add(parameter);
            }

            ExecuteNonQuery();
        }

        /// <summary>
        ///     Returns the specified entity contract with given entityContract
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        /// <returns></returns>
        public void SelectEntity(object entityContract)
        {
            var result = GetGenerator().GenerateSelectStatementFromEntityContract(entityContract);

            foreach (var parameter in result.GeneratedParameters)
            {
                _command.Parameters.Add(parameter);
            }

            var reader = ExecuteReader();
            foreach (var property in entityContract.GetType().GetProperties().Where(p => p.GetCustomAttribute<DbColumnAttribute>() != null))
            {
                var value = reader[property.Name];
                property.SetValue(entityContract, value);
            }
        }
        #endregion
    }
}