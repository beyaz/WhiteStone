using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The SQL command dumper
    /// </summary>
    public static class SqlCommandDumper
    {
        #region Static Fields
        /// <summary>
        ///     The format culture
        /// </summary>
        static readonly CultureInfo FormatCulture = CultureInfo.InvariantCulture;
        #endregion

        #region Public Methods
        /// <summary>
        ///     Ases the executable SQL statement.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string AsExecutableSqlStatement(this SqlCommand command)
        {
            return CreateExecutableSqlStatement(command.CommandText, command.CommandType, command.Parameters.ToList());
        }

        /// <summary>
        ///     Replaces a single quote to a double single quote.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">sql</exception>
        public static string ReplaceSingleQuote(this string sql)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }

            return sql.Replace("'", "''");
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Converts to parameters.
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        internal static SqlParameter[] ConvertToParams(IList<SqlParameter> sqlParameters)
        {
            SqlParameter[] parameters = null;
            if (sqlParameters != null)
            {
                parameters = sqlParameters.ToArray();
            }

            return parameters;
        }

        /// <summary>
        ///     Gets the type of the command.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal static CommandType GetCommandType(string sql)
        {
            // Bold assumtion, if sql has no space, then it's a stored procedure.
            return sql.Contains(" ") ? CommandType.Text : CommandType.StoredProcedure;
        }

        /// <summary>
        ///     Creates a T-SQL notation for a SQL Query that can be executed in Sql server management studio.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static string CreateExecutableQueryStatement(string sql, IEnumerable<SqlParameter> parameters)
        {
            string sqlString = string.Empty;
            if (parameters != null)
            {
                foreach (var dbParameter in parameters)
                {
                    sqlString += CreateParameterText(dbParameter);
                }
            }

            sqlString += sql;
            return sqlString;
        }

        /// <summary>
        ///     Creates a SQL-string with the parameter declaration and the sql statement so it can be executed in Sql Server
        ///     Management studio.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">sql</exception>
        /// <exception cref="NotSupportedException"></exception>
        static string CreateExecutableSqlStatement(string sql, CommandType commandType, IEnumerable<SqlParameter> parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }

            var safeParameters = parameters;
            if (safeParameters != null)
            {
                safeParameters = safeParameters.Where(p => p != null).ToArray();
            }

            switch (commandType)
            {
                case CommandType.StoredProcedure:
                    return CreateExecutableStoredProcedureStatement(sql, safeParameters);
                case CommandType.Text:
                    return CreateExecutableQueryStatement(sql, safeParameters);
                default:
                    throw new NotSupportedException(string.Format(FormatCulture, "The command type {0} is not supported.", commandType));
            }
        }

        /// <summary>
        ///     Creates a T-SQL notation for a stored procedure that can be executed in Sql server management studio.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static string CreateExecutableStoredProcedureStatement(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            IList<string> sqlParameters = new List<string>();
            if (parameters != null)
            {
                foreach (var sqlParameter in parameters)
                {
                    string param = string.Format(FormatCulture, "@{0} = {1}", sqlParameter.ParameterName,
                                                 GetParameterValue(sqlParameter));
                    sqlParameters.Add(param);
                }
            }

            string spString = string.Format(FormatCulture, "EXEC {0} {1}", storedProcedureName, string.Join(", ", sqlParameters));
            return spString;
        }

        /// <summary>
        ///     Creates a parameter declaration for a query.
        /// </summary>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        static string CreateParameterText(SqlParameter dbParameter)
        {
            var sql = GetParameterDeclaration(dbParameter);
            sql += " = ";
            sql += GetParameterValue(dbParameter);
            sql += Environment.NewLine;
            return sql;
        }

        /// <summary>
        ///     Formats the declaration of the the sql parameter to text. (declare @myparam int)
        /// </summary>
        /// <param name="dbParameter">The database parameter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dbParameter</exception>
        static string GetParameterDeclaration(SqlParameter dbParameter)
        {
            if (dbParameter == null)
            {
                throw new ArgumentNullException("dbParameter");
            }

            string declare;
            switch (dbParameter.SqlDbType)
            {
                case SqlDbType.NText:
                case SqlDbType.Text:
                    declare = string.Format(FormatCulture, "declare @{0} nvarchar(max)", dbParameter.ParameterName);
                    break;
                case SqlDbType.Udt:
                    declare = string.Format(FormatCulture, "declare @{0} {1}", dbParameter.ParameterName, dbParameter.UdtTypeName);
                    break;
                default:

                    declare = string.Format(FormatCulture, "declare @{0} {1}", dbParameter.ParameterName,
                                            dbParameter.SqlDbType);
                    if (dbParameter.Size > 0)
                    {
                        if (dbParameter.Precision > 0)
                        {
                            declare += string.Format(FormatCulture, "({0},{1})", dbParameter.Size, dbParameter.Precision);
                        }
                        else
                        {
                            declare += string.Format(FormatCulture, "({0})", dbParameter.Size);
                        }
                    }

                    break;
            }

            return declare;
        }

        /// <summary>
        ///     Formats the value of the the sql parameter to text.
        /// </summary>
        /// <param name="sqlParameter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">sqlParameter</exception>
        /// <exception cref="NotImplementedException"></exception>
        static string GetParameterValue(SqlParameter sqlParameter)
        {
            if (sqlParameter == null)
            {
                throw new ArgumentNullException("sqlParameter");
            }

            string retval;

            if (sqlParameter.Value == DBNull.Value)
            {
                return "null";
            }

            switch (sqlParameter.SqlDbType)
            {
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                    retval = string.Format(FormatCulture, "N'{0}'", sqlParameter.Value.ToString().ReplaceSingleQuote());
                    break;
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                case SqlDbType.Time:
                case SqlDbType.UniqueIdentifier:
                    retval = string.Format(FormatCulture, "'{0}'", sqlParameter.Value.ToString().ReplaceSingleQuote());
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    var dateTime = ((DateTime) sqlParameter.Value).ToString("yyyy-MM-dd HH:mm:ss:fff", FormatCulture);
                    retval = string.Format(FormatCulture, "convert(datetime,'{0}', 121)", dateTime);
                    break;
                case SqlDbType.Bit:
                    retval = bool.Parse(sqlParameter.Value.ToString()) ? "1" : "0";
                    break;
                case SqlDbType.Decimal:
                    retval = ((decimal) sqlParameter.Value).ToString(FormatCulture);
                    break;
                case SqlDbType.Image:
                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                    retval = " -- The image and binary data types are not supported --";
                    break;
                case SqlDbType.Udt:
                    throw new NotImplementedException();
                // retval = "udt";
                //if (sqlParameter.UdtTypeName.Equals("geometry", StringComparison.OrdinalIgnoreCase))
                //{
                //    var geometry = sqlParameter.Value as SqlGeometry;
                //    retval = string.Format(FormatCulture, "geometry::STGeomFromText('{0}',{1})", sqlParameter.Value, geometry.STSrid);
                //}

                //if (sqlParameter.UdtTypeName.Equals("geography", StringComparison.OrdinalIgnoreCase))
                //{
                //    var sqlGeography = sqlParameter.Value as SqlGeography;
                //    retval = string.Format(FormatCulture, "geography::STGeomFromText('{0}',{1})", sqlParameter.Value, sqlGeography.STSrid);
                //}
                // break;
                default:
                    retval = sqlParameter.Value.ToString().ReplaceSingleQuote();
                    break;
            }

            return retval;
        }

        /// <summary>
        ///     To the list.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        static List<SqlParameter> ToList(this SqlParameterCollection collection)
        {
            var list = new List<SqlParameter>();

            foreach (SqlParameter p in collection)
            {
                list.Add(p);
            }

            return list;
        }
        #endregion
    }
}