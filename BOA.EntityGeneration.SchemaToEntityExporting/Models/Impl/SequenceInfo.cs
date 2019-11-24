﻿using System;
using BOA.EntityGeneration.Models.Interfaces;

namespace BOA.EntityGeneration.Models.Impl
{
    /// <summary>
    ///     The sequence information
    /// </summary>
    [Serializable]
    public class SequenceInfo : ISequenceInfo
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