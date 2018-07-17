using RavSoft.GoogleTranslator;

namespace BOA.LanguageTranslations.Google
{
    /// <summary>
    ///     The english to turkish translator
    /// </summary>
    public class EnglishToTurkishTranslator
    {
        #region Public Methods
        /// <summary>
        ///     Translates the specified source text in english.
        /// </summary>
        public static string Translate(string sourceTextInEnglish)
        {
            var t = new Translator
            {
                SourceLanguage = "English",
                TargetLanguage = "Turkish",
                SourceText     = sourceTextInEnglish
            };
            t.Translate();
            return t.Translation;
        }
        #endregion
    }
}