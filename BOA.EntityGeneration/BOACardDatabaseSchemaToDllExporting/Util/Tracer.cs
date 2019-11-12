using System;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    /// <summary>
    ///     The process information
    /// </summary>
    [Serializable]
    public class ProcessInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the current.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        ///     Gets the percentage of completion.
        /// </summary>
        public int PercentageOfCompletion
        {
            get
            {
                if (Current == 0)
                {
                    return 0;
                }

                return (int) (Current / (double) Total * 100);
            }
        }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Gets or sets the total.
        /// </summary>
        public int Total { get; set; }
        #endregion
    }

    /// <summary>
    ///     The tracer
    /// </summary>
    public class Tracer
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets all schema generation process.
        /// </summary>
        public ProcessInfo AllSchemaGenerationProcess { get; set; } = new ProcessInfo();

        /// <summary>
        ///     Gets or sets the custom SQL generation of profile identifier process.
        /// </summary>
        public ProcessInfo CustomSqlGenerationOfProfileIdProcess { get; set; } = new ProcessInfo();

        /// <summary>
        ///     Gets or sets the schema generation process.
        /// </summary>
        public ProcessInfo SchemaGenerationProcess { get; set; } = new ProcessInfo();
        #endregion
    }
}