using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using BOA.Data.Attributes;

namespace BOA.Data
{
    /// <summary>
    ///     The DML SQL generator
    /// </summary>
    class DmlSqlGenerator
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DmlSqlGenerator" /> class.
        /// </summary>
        public DmlSqlGenerator()
        {
            CreateDbDataParameter = CreateSqlParameter;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the create database data parameter.
        /// </summary>
        public Func<string, object, IDbDataParameter> CreateDbDataParameter { get; set; }

        /// <summary>
        ///     Gets prefix of sql parameters.
        /// </summary>
        public string ParameterPrefix { get; set; } = "@";
        #endregion

        #region Public Methods
        /// <summary>
        ///     Generates the delete statement from given entity contract.
        /// </summary>
        public DmlSqlGeneratorOutput GenerateDeleteStatementFromEntityContract(object instance)
        {
            var output = new DmlSqlGeneratorOutput();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("DELETE FROM ");
            sb.Append("[");
            sb.Append(schemaName);
            sb.Append("]");
            sb.Append(".");
            sb.Append("[");
            sb.Append(tableName);
            sb.Append("]");
            sb.Append(" WHERE ");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbPrimaryKeyAttribute>() != null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => "[" + p.ParameterName + "]" + " = " + ParameterPrefix + p.ParameterName)));

            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();

            return output;
        }

        /// <summary>
        ///     Generates the INSERT statement from given entity contract.
        /// </summary>
        public DmlSqlGeneratorOutput GenerateInsertStatementFromEntityContract(object instance)
        {
            var output = new DmlSqlGeneratorOutput();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("INSERT INTO ");
            sb.Append("[");
            sb.Append(schemaName);
            sb.Append("]");
            sb.Append(".");
            sb.Append("[");
            sb.Append(tableName);
            sb.Append("]");
            sb.Append(" (");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbColumnAttribute>() != null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => "[" + p.ParameterName + "]")));
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
        public DmlSqlGeneratorOutput GenerateSelectStatementFromEntityContract(object instance)
        {
            var output = new DmlSqlGeneratorOutput();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("SELECT * FROM ");
            sb.Append("[");
            sb.Append(schemaName);
            sb.Append("]");
            sb.Append(".");
            sb.Append("[");
            sb.Append(tableName);
            sb.Append("]");
            sb.Append(" WHERE ");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbPrimaryKeyAttribute>() != null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => "[" + p.ParameterName + "]" + " = " + ParameterPrefix + p.ParameterName)));

            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();

            return output;
        }

        /// <summary>
        ///     Generates the update statement from given entity contract.
        /// </summary>
        public DmlSqlGeneratorOutput GenerateUpdateStatementFromEntityContract(object instance)
        {
            var output = new DmlSqlGeneratorOutput();
            var sb = new StringBuilder();
            var parameters = new List<IDbDataParameter>();

            var type = instance.GetType();
            var tableName = type.GetCustomAttribute<DbTableAttribute>()?.Name;
            var schemaName = type.GetCustomAttribute<DbSchemaAttribute>()?.Name;

            sb.Append("UPDATE ");
            sb.Append("[");
            sb.Append(schemaName);
            sb.Append("]");
            sb.Append(".");
            sb.Append("[");
            sb.Append(tableName);
            sb.Append("]");
            sb.Append(" SET ");

            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbColumnAttribute>() != null && p.GetCustomAttribute<DbPrimaryKeyAttribute>() == null))
            {
                parameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", parameters.Select(p => "[" + p.ParameterName + "]" + " = " + ParameterPrefix + p.ParameterName)));

            sb.Append(" WHERE ");

            var whereParameters = new List<IDbDataParameter>();
            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<DbPrimaryKeyAttribute>() != null))
            {
                whereParameters.Add(CreateDbDataParameter(property.Name, property.GetValue(instance)));
            }

            sb.Append(string.Join(",", whereParameters.Select(p => "[" + p.ParameterName + "]" + " = " + ParameterPrefix + p.ParameterName)));

            parameters.AddRange(whereParameters);

            output.GeneratedParameters = parameters;
            output.GenratedSql = sb.ToString();

            return output;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Creates the SQL parameter.
        /// </summary>
        static IDbDataParameter CreateSqlParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }
        #endregion
    }
}