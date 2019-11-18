﻿using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.EntityGeneration.Models.Interfaces;
using ITableInfo = BOA.EntityGeneration.Models.Interfaces.ITableInfo;

namespace BOA.EntityGeneration.Models.Impl
{
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
        ///     Gets or sets the non unique index information list.
        /// </summary>
        public IReadOnlyList<IIndexInfo> NonUniqueIndexInfoList { get; set; }

        /// <summary>
        ///     Gets or sets the sequence list.
        /// </summary>
        public IReadOnlyList<ISequenceInfo> SequenceList { get; set; }

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
}