using RavSoft.GoogleTranslator;

namespace BOA.LanguageTranslations
{
    class Google
    {
        #region Public Methods
        public static string TranslateTurkishToEnglish(string sourceText)
        {
            var t = new Translator
            {
                SourceLanguage = "Turkish",
                TargetLanguage = "English",
                SourceText     = sourceText
            };
            t.Translate();
            return t.Translation;
        }
        #endregion
    }
}