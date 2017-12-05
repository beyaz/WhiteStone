using System;
using System.Data;
using System.Data.SqlClient;

namespace BOA.Data
{
    /// <summary>
    ///     Represents database layer.
    /// </summary>
    public class SqlDatabase : Database
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlDatabase" />
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlDatabase(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlDatabase" />
        /// </summary>
        /// <param name="connection"></param>
        public SqlDatabase(IDbConnection connection)
            : base(connection)
        {
        }

        /// <summary>
        ///     Gets prefix of sql parameters.
        /// </summary>
        public override string ParameterPrefix
        {
            get { return "@"; }
        }

        /// <summary>
        ///     Initializes new <see cref="SqlParameter" /> instance.
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

            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }
    }
}