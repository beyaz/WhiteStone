using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BOA.LanguageTranslations.Google
{
    /// <summary>
    ///     The translator
    /// </summary>
    public static class Translator
    {
        #region Public Methods
        /// <summary>
        ///     Translate2s the specified text2.
        /// </summary>
        public static string Translate(string text, string fromCulture, string toCulture)
        {
            var translation = string.Empty;

            var url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                    fromCulture,
                                    toCulture,
                                    HttpUtility.UrlEncode(text));

            var outputFile = Path.GetTempFileName();
            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                wc.DownloadFile(url, outputFile);
            }

            // Get translated text
            if (File.Exists(outputFile))
            {
                // Get phrase collection
                var responseText = File.ReadAllText(outputFile);
                var index        = responseText.IndexOf(string.Format(",,\"{0}\"", fromCulture), StringComparison.Ordinal);
                if (index == -1)
                {
                    // Translation of single word
                    var startQuote = responseText.IndexOf('\"');
                    if (startQuote != -1)
                    {
                        var endQuote = responseText.IndexOf('\"', startQuote + 1);
                        if (endQuote != -1)
                        {
                            translation = responseText.Substring(startQuote + 1, endQuote - startQuote - 1);
                        }
                    }
                }
                else
                {
                    // Translation of phrase
                    responseText = responseText.Substring(0, index);
                    responseText = responseText.Replace("],[", ",");
                    responseText = responseText.Replace("]", string.Empty);
                    responseText = responseText.Replace("[", string.Empty);
                    responseText = responseText.Replace("\",\"", "\"");

                    // Get translated phrases
                    var phrases = responseText.Split(new[] {'\"'}, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < phrases.Count(); i += 2)
                    {
                        var translatedPhrase = phrases[i];
                        if (translatedPhrase.StartsWith(",,"))
                        {
                            i--;
                            continue;
                        }

                        translation += translatedPhrase + "  ";
                    }
                }

                // Fix up translation
                translation = translation.Trim();
                translation = translation.Replace(" ?", "?");
                translation = translation.Replace(" !", "!");
                translation = translation.Replace(" ,", ",");
                translation = translation.Replace(" .", ".");
                translation = translation.Replace(" ;", ";");
            }

            return translation;
        }

        /// <summary>
        ///     Translates the english to turkish.
        /// </summary>
        public static string TranslateEnglishToTurkish(string sourceTextInEnglish)
        {
            return Translate(sourceTextInEnglish, "en", "tr");
        }

        /// <summary>
        ///     Translates the turkish to english.
        /// </summary>
        public static string TranslateTurkishToEnglish(string sourceTextInTurkish)
        {
            return Translate(sourceTextInTurkish, "tr", "en");
        }
        #endregion
    }
}