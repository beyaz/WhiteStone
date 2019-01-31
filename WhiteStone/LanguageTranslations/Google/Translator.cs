using System.Net;
using System.Text;
using System.Web;
using BOA.Common.Helpers;

namespace BOA.LanguageTranslations.Google
{
    /// <summary>
    ///     The translator
    /// </summary>
    public static class Translator
    {
        #region Public Methods
        /// <summary>
        ///     Translates the specified text.
        /// </summary>
        public static string Translate(string text, string fromCulture, string toCulture)
        {
            fromCulture = fromCulture.ToLower();
            toCulture   = toCulture.ToLower();

            // normalize the culture in case something like en-us was passed 
            // retrieve only en since Google doesn't support sub-locales
            var tokens = fromCulture.Split('-');
            if (tokens.Length > 1)
            {
                fromCulture = tokens[0];
            }

            // normalize ToCulture
            tokens = toCulture.Split('-');
            if (tokens.Length > 1)
            {
                toCulture = tokens[0];
            }

            var url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                    HttpUtility.UrlEncode(text), fromCulture, toCulture);

            // Retrieve Translation with HTTP GET call
            string html = null;

            var web = new WebClient();

            // MUST add a known browser user agent or else response encoding doen't return UTF-8 (WTF Google?)
            web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

            // Make sure we have response encoding to UTF-8
            web.Encoding = Encoding.UTF8;
            html         = web.DownloadString(url);

            return html.Trim().RemoveFromEnd("\"").RemoveFromStart("\"");
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