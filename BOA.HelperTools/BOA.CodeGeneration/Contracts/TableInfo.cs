using System;
using System.Collections.Generic;

namespace BOA.CodeGeneration.Contracts
{
    /// <summary>
    ///     The table information
    /// </summary>
    [Serializable]
    public class TableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        public IReadOnlyList<ColumnInfo> Columns { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has identity column.
        /// </summary>
        public bool HasIdentityColumn { get; set; }

        /// <summary>
        ///     Gets or sets the identity column.
        /// </summary>
        public ColumnInfo IdentityColumn { get; set; }

        /// <summary>
        ///     Gets or sets the primary key columns.
        /// </summary>
        public IReadOnlyList<ColumnInfo> PrimaryKeyColumns { get; set; }
        #endregion
    }

    /// <summary>
    ///     The column information
    /// </summary>
    [Serializable]
    public class ColumnInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the column.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        ///     Gets or sets the type of the dot net.
        /// </summary>
        public string DotNetType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        ///     Gets or sets the name of the SQL database type.
        /// </summary>
        public string SqlDatabaseTypeName { get; set; }

        /// <summary>
        ///     Gets or sets the SQL reader method.
        /// </summary>
        public string SqlReaderMethod { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return DotNetType + " " + ColumnName;
        }
        #endregion
    }
}