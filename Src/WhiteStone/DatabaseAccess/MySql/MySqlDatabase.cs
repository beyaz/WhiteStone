using System;
using System.Data;
using BOA.DatabaseAccess;
using MySql.Data.MySqlClient;

namespace BOA.Data.MySql
{
    /// <summary>
    ///     Represents database layer.
    /// </summary>
    public class MySqlDatabase : Database
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MySqlDatabase" />
        /// </summary>
        /// <param name="connectionString"></param>
        public MySqlDatabase(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MySqlDatabase" />
        /// </summary>
        /// <param name="connection"></param>
        public MySqlDatabase(IDbConnection connection)
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
        ///     Initializes new <see cref="MySqlParameter" /> instance.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override IDbDataParameter CreateParameter(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            return new MySqlParameter(parameterName, value ?? DBNull.Value);
        }
        #endregion
    }
}