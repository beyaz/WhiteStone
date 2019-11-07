namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models
{
    /// <summary>
    ///     The sequence information
    /// </summary>
    public interface ISequenceInfo
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get;  }

        /// <summary>
        ///     Gets or sets the name of the target column.
        /// </summary>
        string TargetColumnName { get;  }
    }
}