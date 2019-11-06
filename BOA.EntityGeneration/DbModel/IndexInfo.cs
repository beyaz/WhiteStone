using System;
using System.Collections.Generic;
using System.Linq;

namespace BOA.EntityGeneration.DbModel
{
    /// <summary>
    ///     The index information
    /// </summary>
    public interface IIndexInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the column names.
        /// </summary>
        IReadOnlyList<string> ColumnNames { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is clustered.
        /// </summary>
        bool IsClustered { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is non clustered.
        /// </summary>
        bool IsNonClustered { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        bool IsPrimaryKey { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        bool IsUnique { get; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get; }
        #endregion
    }

    /// <summary>
    ///     The index information
    /// </summary>
    [Serializable]
    public sealed class IndexInfo : IIndexInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the column names.
        /// </summary>
        public IReadOnlyList<string> ColumnNames { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is clustered.
        /// </summary>
        public bool IsClustered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is non clustered.
        /// </summary>
        public bool IsNonClustered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            var properties = new List<string>();

            if (IsPrimaryKey)
            {
                properties.Add("PrimaryKey");
            }

            if (IsUnique)
            {
                properties.Add("Unique");
            }

            if (IsClustered)
            {
                properties.Add("Clustered");
            }

            if (IsNonClustered)
            {
                properties.Add("NonClustered");
            }

            return $"[ {string.Join(" + ", properties)} ] index on {string.Join(" and ", ColumnNames.Select(x => '"' + x + '"'))}";
        }
        #endregion
    }
}