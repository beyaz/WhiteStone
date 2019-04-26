﻿using System;
using System.Collections.Generic;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Generators
{
    /// <summary>
    ///     The generator data
    /// </summary>
    [Serializable]
    public class GeneratorData
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support get all.
        /// </summary>
        public bool IsSupportGetAll { get; set; }

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
        ///     Gets or sets the namespace full name of type assembly.
        /// </summary>
        public string NamespaceFullNameOfTypeAssembly { get; set; }

        /// <summary>
        ///     Gets or sets the non unique index identifiers.
        /// </summary>
        public IReadOnlyList<IndexIdentifier> NonUniqueIndexIdentifiers { get; set; }

        /// <summary>
        ///     Gets or sets the name of the sequence.
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        ///     Gets or sets the table information.
        /// </summary>
        public TableInfo TableInfo { get; set; }

        /// <summary>
        ///     Gets or sets the unique index identifiers.
        /// </summary>
        public IReadOnlyList<IndexIdentifier> UniqueIndexIdentifiers { get; set; }
        #endregion
    }
}