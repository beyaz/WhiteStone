using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.DbModel
{
    /// <summary>
    ///     The table information
    /// </summary>
    [Serializable]
    public class TableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the catalog.
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns { get; set; }

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
        public IReadOnlyList<IndexInfo> IndexInfoList { get; set; }

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