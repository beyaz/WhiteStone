using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using BOA.Data;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     Manager of sql operations
    /// </summary>
    public abstract class Database : IDatabase
    {
        #region Fields
        IDbCommand _command;
        IDbConnection _connection;
        IDbTransaction _transaction;
        #endregion


        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;


        #region Public Properties
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

        /// <summary>
        ///     The time (in seconds) to wait for the command to execute. The default value 30 seconds in .net
        ///     <para>  value is null  </para>
        /// </summary>
        public int? CommandTimeout { set; get; }

        /// <summary>
        ///     Gets prefix of sql parameters.
        /// </summary>
        public abstract string ParameterPrefix { get; }
        #endregion

        #region Public Indexers
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
        #endregion

        #region Public Methods
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
        ///     if has any transaction then rollback transaction.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
        #endregion

        #region Methods
        /// <summary>
        ///     Initializes new <see cref="IDbDataParameter" /> instance.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract IDbDataParameter CreateParameter(string parameterName, object value);

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

        IDbCommand CreateCommand()
        {
            var c = _connection.CreateCommand();
            if (CommandTimeout != null)
            {
                c.CommandTimeout = (int)CommandTimeout;
            }
            if (_transaction != null)
            {
                c.Transaction = _transaction;
            }
            c.CommandType = CommandType.Text; // default
            return c;
        }

     

        void OpenConnection(IDbConnection connection)
        {
            _connection = connection;
            if (connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }
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

        
    }
}