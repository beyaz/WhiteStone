using System;
using System.Data;
using System.Data.SqlClient;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     Represents database layer.
    /// </summary>
    public class SqlDatabase : Database
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlDatabase" />
        /// </summary>
        public SqlDatabase(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlDatabase" />
        /// </summary>
        public SqlDatabase(IDbConnection connection)
            : base(connection)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets prefix of sql parameters.
        /// </summary>
        public override string ParameterPrefix
        {
            get { return "@"; }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Initializes new <see cref="SqlParameter" /> instance.
        /// </summary>
        protected override IDbDataParameter CreateParameter(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }
        #endregion
    }
}