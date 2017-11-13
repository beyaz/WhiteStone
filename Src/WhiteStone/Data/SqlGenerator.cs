using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using WhiteStone.Data.Attributes;

namespace WhiteStone.Data
{
    /// <summary>
    /// </summary>
    class SqlGenerator
    {
        /// <summary>
        ///     Gets prefix of sql parameters.
        /// </summary>
        public string ParameterPrefix { get; set; } = "@";

        /// <summary>
        ///     Gets or sets the create database data parameter.
        /// </summary>
        /// <value>
        ///     The create database data parameter.
        /// </value>
        public Func<string, object, IDbDataParameter> CreateDbDataParameter { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlGenerator" /> class.
        /// </summary>
        public SqlGenerator()
        {
            CreateDbDataParameter = CreateSqlParameter;
        }

        /// <summary>
        ///     Creates the SQL parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        static IDbDataParameter CreateSqlParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }


        /// <summary>
        /// </summary>
        public class Output
        {
            /// <summary>
            ///     Gets or sets the genrated SQL.
            /// </summary>
            /// <value>
            ///     The genrated SQL.
            /// </value>
            public string GenratedSql { get; set; }

            /// <summary>
            ///     Gets or sets the generated parameters.
            /// </summary>
            /// <value>
            ///     The generated parameters.
            /// </value>
            public List<IDbDataParameter> GeneratedParameters { get; set; }
        }


        /// <summary>
        ///     Generates the INSERT statement from given entity contract.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public Output GenerateInsertStatementFromEntityContract(object instance)
        {
            var output = new Output();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("INSERT INTO ");
            sb.Append(schemaName);
            sb.Append(".");
            sb.Append(tableName);
            sb.Append(" (");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbColumnAttribute>() != null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => p.ParameterName)));
            sb.Append(")");
            sb.Append(" VALUES (");

            sb.Append(string.Join(",", parameters.Select(p => ParameterPrefix + p.ParameterName)));
            sb.Append(")");

            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();


            return output;
        }


        /// <summary>
        ///     Generates the SELECT statement from given entity contract.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public Output GenerateSelectStatementFromEntityContract(object instance)
        {
            var output = new Output();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("SELECT * FROM ");
            sb.Append(schemaName);
            sb.Append(".");
            sb.Append(tableName);
            sb.Append(" WHERE ");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbPrimaryKeyAttribute>() != null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => p.ParameterName + " = " + ParameterPrefix + p.ParameterName)));


            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();


            return output;
        }

        /// <summary>
        ///     Generates the delete statement from given entity contract.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public Output GenerateDeleteStatementFromEntityContract(object instance)
        {
            var output = new Output();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("DELETE FROM ");
            sb.Append(schemaName);
            sb.Append(".");
            sb.Append(tableName);
            sb.Append(" WHERE ");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbPrimaryKeyAttribute>() != null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => p.ParameterName + " = " + ParameterPrefix + p.ParameterName)));


            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();


            return output;
        }

        /// <summary>
        ///     Generates the update statement from given entity contract.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public Output GenerateUpdateStatementFromEntityContract(object instance)
        {
            var output = new Output();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("UPDATE ");
            sb.Append(schemaName);
            sb.Append(".");
            sb.Append(tableName);
            sb.Append(" SET ");


            foreach (var property in type.GetProperties().Where(p => (p.GetCustomAttribute<DbColumnAttribute>() != null) && (p.GetCustomAttribute<DbPrimaryKeyAttribute>() == null)))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => p.ParameterName + " = " + ParameterPrefix + p.ParameterName)));

            sb.Append(" WHERE ");

            var whereParameters = new List<IDbDataParameter>();
            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbPrimaryKeyAttribute>() != null))
            {
                whereParameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", whereParameters.Select(p => p.ParameterName + " = " + ParameterPrefix + p.ParameterName)));

            parameters.AddRange(whereParameters);


            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();

            return output;
        }
    }
}