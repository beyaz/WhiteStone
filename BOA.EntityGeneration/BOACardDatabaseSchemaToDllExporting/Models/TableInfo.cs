using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models
{
    /// <summary>
    ///     The table information
    /// </summary>
    public interface ITableInfo:BOA.EntityGeneration.DbModel.ITableInfo
    {
        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        string DatabaseEnumName { get;  }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by index.
        /// </summary>
        bool IsSupportSelectByIndex { get;  }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by key.
        /// </summary>
        bool IsSupportSelectByKey { get;  }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by unique index.
        /// </summary>
        bool IsSupportSelectByUniqueIndex { get;  }

        /// <summary>
        ///     Gets or sets the non unique index information list.
        /// </summary>
        IReadOnlyList<IndexInfo> NonUniqueIndexInfoList { get;  }

        /// <summary>
        ///     Gets or sets the sequence list.
        /// </summary>
        IReadOnlyList<SequenceInfo> SequenceList { get;  }

        /// <summary>
        ///     Gets or sets the unique index information list.
        /// </summary>
        IReadOnlyList<IndexInfo> UniqueIndexInfoList { get;  }

        /// <summary>
        ///     Gets or sets a value indicating whether [should generate select all by valid flag method in business class].
        /// </summary>
        bool ShouldGenerateSelectAllByValidFlagMethodInBusinessClass { get;  }

        /// <summary>
        ///     Gets or sets the name of the catalog.
        /// </summary>
        string CatalogName { get;  }

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        IReadOnlyList<IColumnInfo> Columns { get;  }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has identity column.
        /// </summary>
        bool HasIdentityColumn { get;  }

        /// <summary>
        ///     Gets or sets the identity column.
        /// </summary>
        IColumnInfo IdentityColumn { get;  }

        /// <summary>
        ///     Gets or sets the index information list.
        /// </summary>
        IReadOnlyList<IndexInfo> IndexInfoList { get;  }

        /// <summary>
        ///     Gets or sets the primary key columns.
        /// </summary>
        IReadOnlyList<IColumnInfo> PrimaryKeyColumns { get;  }

        /// <summary>
        ///     Gets or sets the name of the schema.
        /// </summary>
        string SchemaName { get;  }

        /// <summary>
        ///     Gets or sets the name of the table.
        /// </summary>
        string TableName { get;  }
    }

    /// <summary>
    ///     The generator data
    /// </summary>
    [Serializable]
    public class TableInfo : DbModel.TableInfo, ITableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by index.
        /// </summary>
        public bool IsSupportSelectByIndex { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by key.
        /// </summary>
        public bool IsSupportSelectByKey { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by unique index.
        /// </summary>
        public bool IsSupportSelectByUniqueIndex { get; set; }

        /// <summary>
        ///     Gets or sets the non unique index information list.
        /// </summary>
        public IReadOnlyList<IndexInfo> NonUniqueIndexInfoList { get; set; }


        /// <summary>
        ///     Gets or sets the sequence list.
        /// </summary>
        public IReadOnlyList<SequenceInfo> SequenceList { get; set; }

        /// <summary>
        ///     Gets or sets the unique index information list.
        /// </summary>
        public IReadOnlyList<IndexInfo> UniqueIndexInfoList { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [should generate select all by valid flag method in business class].
        /// </summary>
        public bool ShouldGenerateSelectAllByValidFlagMethodInBusinessClass { get; set; }
        #endregion
    }

    /// <summary>
    ///     The sequence information
    /// </summary>
    [Serializable]
    public class SequenceInfo
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Gets or sets the name of the target column.
        /// </summary>
        public string TargetColumnName { get; set; }
    }

}