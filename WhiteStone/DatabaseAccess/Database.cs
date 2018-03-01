using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     The database
    /// </summary>
    public abstract class Database : IDatabase
    {
        #region Fields
        /// <summary>
        ///     The command
        /// </summary>
        IDbCommand _command;

        /// <summary>
        ///     The connection
        /// </summary>
        IDbConnection _connection;

        /// <summary>
        ///     The transaction
        /// </summary>
        IDbTransaction _transaction;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Database" /> class.
        /// </summary>
        protected Database(string connectionString)
            : this(new SqlConnection(connectionString))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Database" /> class.
        /// </summary>
        protected Database(IDbConnection connection)
        {
            OpenConnection(connection);
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Indicates CommandType is StoredProcedure or Text
        /// </summary>
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

        /// <summary>
        ///     Command that will be execute
        /// </summary>
        public string CommandText
        {
            set
            {
                _command             = CreateCommand();
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

        /// <summary>
        ///     Command timeout value
        /// </summary>
        public int? CommandTimeout { set; get; }

        /// <summary>
        ///     Gets the connection.
        /// </summary>
        public IDbConnection Connection => _connection;

        /// <summary>
        ///     Parameter prefix for sql sentences.
        /// </summary>
        public abstract string ParameterPrefix { get; }

        /// <summary>
        ///     Gets the transaction.
        /// </summary>
        public IDbTransaction Transaction => _transaction;
        #endregion

        #region Public Indexers
        /// <summary>
        ///     Gets or sets the <see cref="System.Object" /> with the specified parameter name.
        /// </summary>
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
        ///     Begins database transaction
        /// </summary>
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        ///     Commits transaction
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
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Executes command and returns effected row counts
        /// </summary>
        public int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Executes command and returns reader
        /// </summary>
        public IDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }

        /// <summary>
        ///     Executes commands ands returns command value.
        /// </summary>
        public object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }

        /// <summary>
        ///     Rollback transaction
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
        ///     Creates the parameter.
        /// </summary>
        protected abstract IDbDataParameter CreateParameter(string parameterName, object value);

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
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

        /// <summary>
        ///     Creates the command.
        /// </summary>
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

            c.CommandType = CommandType.Text;
            return c;
        }

        /// <summary>
        ///     Opens the connection.
        /// </summary>
        void OpenConnection(IDbConnection connection)
        {
            _connection = connection;
            if (connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }
        #endregion
    }
}