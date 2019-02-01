using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BOA.Common.Helpers;
using WhiteStone.Helpers;

namespace BOA.LanguageTranslations.Yandex
{
    /// <summary>
    ///     The translator
    /// </summary>
    public class Translator
    {
        #region Constants
        /// <summary>
        ///     The API key
        /// </summary>
        const string _apiKey = "trnsl.1.1.20190131T125909Z.2bd57526ba6f9951.472ab0daa7a7700c7cc8cd95d9613bc2f994d659";
        #endregion

        #region Public Methods
        /// <summary>
        ///     Translates the specified language.
        /// </summary>
        public static string Translate(string text, string lang)
        {
            var requestString = string.Format("https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&text={1}&lang={2}&format={3}",
                                              _apiKey, text, lang, "plain");

            var request = WebRequest.Create(requestString);
            if (requestString.Length > 10240 && request.Method.StartsWith("GET"))
            {
                throw new ArgumentException("Text is too long (>10Kb)");
            }

            var response = request.GetResponse();

            var translateData = JsonHelper.Deserialize<TranslateData>(response.GetResponseStream().ReadToEndAsString());

            return translateData.text.FirstOrDefault();
        }

        /// <summary>
        ///     Translates the english to turkish.
        /// </summary>
        public static string TranslateEnglishToTurkish(string sourceTextInEnglish)
        {
            return Translate(sourceTextInEnglish, "en-tr");
        }

        /// <summary>
        ///     Translates the turkish to english.
        /// </summary>
        public static string TranslateTurkishToEnglish(string sourceTextInTurkish)
        {
            return Translate(sourceTextInTurkish, "tr-en");
        }
        #endregion

        /// <summary>
        ///     The translate data
        /// </summary>
        class TranslateData
        {
            #region Public Properties
            /// <summary>
            ///     Gets or sets the code.
            /// </summary>
            public int code { get; set; }

            /// <summary>
            ///     Gets or sets the language.
            /// </summary>
            public string lang { get; set; }

            /// <summary>
            ///     Gets or sets the text.
            /// </summary>
            public List<string> text { get; set; }
            #endregion
        }
    }
}