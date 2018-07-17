using RavSoft.GoogleTranslator;

namespace BOA.LanguageTranslations.Google
{
    /// <summary>
    ///     The turkish to english translator
    /// </summary>
    public class TurkishToEnglishTranslator
    {
        #region Public Methods
        /// <summary>
        ///     Translates the specified source text in turkish.
        /// </summary>
        public static string Translate(string sourceTextInTurkish)
        {
            var t = new Translator
            {
                SourceLanguage = "Turkish",
                TargetLanguage = "English",
                SourceText     = sourceTextInTurkish
            };
            t.Translate();
            return t.Translation;
        }
        #endregion
    }
}