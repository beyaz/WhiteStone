using System;
using System.Data;

namespace BOA.EntityGeneration.DbModel
{
    /// <summary>
    ///     The column information
    /// </summary>
    public interface IColumnInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the column.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        string Comment { get; }

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        string DataType { get; }

        /// <summary>
        ///     Gets or sets the type of the dot net.
        /// </summary>
        string DotNetType { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        bool IsIdentity { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        bool IsPrimaryKey { get; }

        /// <summary>
        ///     Gets or sets the name of the SQL database type.
        /// </summary>
        SqlDbType SqlDbType { get; }

        /// <summary>
        ///     Gets or sets the SQL reader method.
        /// </summary>
        SqlReaderMethods SqlReaderMethod { get; }
        #endregion
    }

    /// <summary>
    ///     The column information
    /// </summary>
    [Serializable]
    public sealed class ColumnInfo : IColumnInfo
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
        public SqlDbType SqlDbType { get; set; }

        /// <summary>
        ///     Gets or sets the SQL reader method.
        /// </summary>
        public SqlReaderMethods SqlReaderMethod { get; set; }
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