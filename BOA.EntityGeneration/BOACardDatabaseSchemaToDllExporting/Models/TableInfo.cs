using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.Interfaces;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models
{
    /// <summary>
    ///     The table information
    /// </summary>
    public interface ITableInfo : DbModel.Interfaces.ITableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        string DatabaseEnumName { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by index.
        /// </summary>
        bool IsSupportSelectByIndex { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by key.
        /// </summary>
        bool IsSupportSelectByKey { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by unique index.
        /// </summary>
        bool IsSupportSelectByUniqueIndex { get; }

        /// <summary>
        ///     Gets or sets the non unique index information list.
        /// </summary>
        IReadOnlyList<IIndexInfo> NonUniqueIndexInfoList { get; }

        /// <summary>
        ///     Gets or sets the sequence list.
        /// </summary>
        IReadOnlyList<SequenceInfo> SequenceList { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether [should generate select all by valid flag method in business class].
        /// </summary>
        bool ShouldGenerateSelectAllByValidFlagMethodInBusinessClass { get; }

        /// <summary>
        ///     Gets or sets the unique index information list.
        /// </summary>
        IReadOnlyList<IIndexInfo> UniqueIndexInfoList { get; }
        #endregion
    }

    /// <summary>
    ///     The generator data
    /// </summary>
    [Serializable]
    public class TableInfo : DbModel.Types.TableInfo, ITableInfo
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
        public IReadOnlyList<IIndexInfo> NonUniqueIndexInfoList { get; set; }

        /// <summary>
        ///     Gets or sets the sequence list.
        /// </summary>
        public IReadOnlyList<SequenceInfo> SequenceList { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [should generate select all by valid flag method in business class].
        /// </summary>
        public bool ShouldGenerateSelectAllByValidFlagMethodInBusinessClass { get; set; }

        /// <summary>
        ///     Gets or sets the unique index information list.
        /// </summary>
        public IReadOnlyList<IIndexInfo> UniqueIndexInfoList { get; set; }
        #endregion
    }

    /// <summary>
    ///     The sequence information
    /// </summary>
    [Serializable]
    public class SequenceInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the name of the target column.
        /// </summary>
        public string TargetColumnName { get; set; }
        #endregion
    }
}