using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BOA.Common.Helpers;
using HtmlAgilityPack;

namespace BOA.LanguageTranslations
{
    class Longman
    {
        #region Public Methods
        public static LongmanWordInfo GetWordInfo(string englishWord)
        {
            if (englishWord.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(nameof(englishWord));
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var url = "https://www.ldoceonline.com/dictionary/" + englishWord;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            return new LongmanWordInfo
            {
                Dictentries = doc.DocumentNode.GetElementbyClass("dictentry").Select(ParseDictentry).ToList()
            };
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Parses the dictentry.
        /// </summary>
        static Dictentry ParseDictentry(HtmlNode dictentry)
        {
            var result = new Dictentry
            {
                Topics = dictentry.GetElementbyClass("topic").Select(e => e.InnerHtml).ToList()
            };

            return result;
        }
        #endregion
    }

    /// <summary>
    ///     The dictentry
    /// </summary>
    [Serializable]
    public class Dictentry
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the topics.
        /// </summary>
        public List<string> Topics { get; set; }
        #endregion
    }

    /// <summary>
    ///     The longman word information
    /// </summary>
    [Serializable]
    public class LongmanWordInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the dictentries.
        /// </summary>
        public List<Dictentry> Dictentries { get; set; }
        #endregion
    }
}