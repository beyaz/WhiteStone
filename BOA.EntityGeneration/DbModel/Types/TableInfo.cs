using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.DbModel.Types
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
        public IReadOnlyList<ColumnInfo> Columns { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has identity column.
        /// </summary>
        public bool HasIdentityColumn { get; set; }

        /// <summary>
        ///     Gets or sets the identity column.
        /// </summary>
        public ColumnInfo IdentityColumn { get; set; }

        public IReadOnlyList<IndexInfo> IndexInfoList { get; set; }

        /// <summary>
        ///     Gets or sets the primary key columns.
        /// </summary>
        public IReadOnlyList<ColumnInfo> PrimaryKeyColumns { get; set; }

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