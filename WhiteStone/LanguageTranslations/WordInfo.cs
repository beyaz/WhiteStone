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
        public string Explanation { get; set; }
        #region Public Properties
        /// <summary>
        ///     Gets or sets the sample sentences.
        /// </summary>
        public List<string> SampleSentences { get; set; }

        /// <summary>
        ///     Gets or sets the turkish pronanciation.
        /// </summary>
        public string TurkishPronanciation { get; set; }
        #endregion
    }



}