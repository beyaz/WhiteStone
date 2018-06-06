using System;
using System.Collections.Generic;

namespace BOA.LanguageTranslations
{
    /// <summary>
    ///     The word information
    /// </summary>
    [Serializable]
    public class WordInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the sample sentences.
        /// </summary>
        public List<MeanInfo> Means { get; set; }

        /// <summary>
        ///     Gets or sets the turkish pronanciation.
        /// </summary>
        public string TurkishPronanciation { get; set; }

        /// <summary>
        ///     Gets or sets the word.
        /// </summary>
        public string Word { get; set; }
        #endregion
    }

    /// <summary>
    ///     The word information
    /// </summary>
    [Serializable]
    public class MeanInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the definition.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        ///     Gets or sets the sample sentences.
        /// </summary>
        public List<string> SampleSentences { get; set; }
        #endregion
    }
}