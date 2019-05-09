﻿using System;
using System.Data;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models
{
    /// <summary>
    ///     The custom SQL information parameter
    /// </summary>
    [Serializable]
    public class CustomSqlInfoParameter
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the c sharp property.
        /// </summary>
        public string CSharpPropertyName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the c sharp property type.
        /// </summary>
        public string CSharpPropertyTypeName { get; set; }

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the name of the SQL database type.
        /// </summary>
        public SqlDbType SqlDbTypeName { get; set; }
        #endregion
    }
}