﻿using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.DbModel
{
    /// <summary>
    ///     The table information
    /// </summary>
    public interface ITableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the catalog.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        IReadOnlyList<IColumnInfo> Columns { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has identity column.
        /// </summary>
        bool HasIdentityColumn { get; }

        /// <summary>
        ///     Gets or sets the identity column.
        /// </summary>
        IColumnInfo IdentityColumn { get; }

        /// <summary>
        ///     Gets or sets the index information list.
        /// </summary>
        IReadOnlyList<IIndexInfo> IndexInfoList { get; }

        /// <summary>
        ///     Gets or sets the primary key columns.
        /// </summary>
        IReadOnlyList<IColumnInfo> PrimaryKeyColumns { get; }

        /// <summary>
        ///     Gets or sets the name of the schema.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        ///     Gets or sets the name of the table.
        /// </summary>
        string TableName { get; }
        #endregion
    }

    /// <summary>
    ///     The table information
    /// </summary>
    [Serializable]
    public class TableInfo : ITableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the catalog.
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        public IReadOnlyList<ColumnInfo> Columns { get; set; }

        IReadOnlyList<IColumnInfo> ITableInfo.Columns => Columns;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has identity column.
        /// </summary>
        public bool HasIdentityColumn { get; set; }

        /// <summary>
        ///     Gets or sets the identity column.
        /// </summary>
        public IColumnInfo IdentityColumn { get; set; }

        /// <summary>
        ///     Gets or sets the index information list.
        /// </summary>
        public IReadOnlyList<IIndexInfo> IndexInfoList { get; set; }

        /// <summary>
        ///     Gets or sets the primary key columns.
        /// </summary>
        public IReadOnlyList<IColumnInfo> PrimaryKeyColumns { get; set; }

        /// <summary>
        ///     Gets or sets the name of the schema.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the table.
        /// </summary>
        public string TableName { get; set; }
        #endregion
    }
}