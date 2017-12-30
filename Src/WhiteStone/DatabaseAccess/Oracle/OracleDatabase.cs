﻿using System;
using System.Data;
using BOA.DatabaseAccess;
using Oracle.DataAccess.Client;

namespace BOA.Data.Oracle
{
    /// <summary>
    ///     Represents database layer.
    /// </summary>
    public class OracleDatabase : Database
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="OracleDatabase" />
        /// </summary>
        /// <param name="connectionString"></param>
        public OracleDatabase(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OracleDatabase" />
        /// </summary>
        /// <param name="connection"></param>
        public OracleDatabase(IDbConnection connection)
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
            get { return ":"; }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Initializes new <see cref="OracleParameter" /> instance.
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

            return new OracleParameter(parameterName, value ?? DBNull.Value);
        }
        #endregion
    }
}