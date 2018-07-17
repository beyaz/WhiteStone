﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BOA.Common.Helpers;
using HtmlAgilityPack;

namespace BOA.LanguageTranslations
{
    /// <summary>
    ///     The longman
    /// </summary>
    class Longman
    {
        #region Public Methods
        /// <summary>
        ///     Gets the word information.
        /// </summary>
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
                Topics = dictentry.GetElementbyClass("topic").Select(e => e.InnerHtml).ToList(),
                Usages = dictentry.GetElementbyClass("newline Sense").Select(ParseUsageInfo).ToList()
            };

            return result;
        }

        /// <summary>
        ///     Parses the example.
        /// </summary>
        static Example ParseExample(HtmlNode example)
        {
            return new Example
            {
                Text          = example.InnerText.Trim(),
                MediaFilePath = example.SelectNodes("span")?.FirstOrDefault()?.GetAttributeValue("data-src-mp3", "")?.Trim()
            };
        }

        /// <summary>
        ///     Parses the usage information.
        /// </summary>
        static UsageInfo ParseUsageInfo(HtmlNode newline_Sense)
        {
            return new UsageInfo
            {
                ShortDefinition = newline_Sense.GetElementbyClass("SIGNPOST").FirstOrDefault()?.InnerHtml,
                FullDefinition  = newline_Sense.GetElementbyClass("DEF").FirstOrDefault()?.InnerText,
                Examples        = newline_Sense.GetElementbyClass("EXAMPLE").Select(ParseExample).ToList()
            };
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

        /// <summary>
        ///     Gets or sets the usages.
        /// </summary>
        public List<UsageInfo> Usages { get; set; }
        #endregion
    }

    /// <summary>
    ///     The usage information
    /// </summary>
    [Serializable]
    public class UsageInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the examples.
        /// </summary>
        public List<Example> Examples { get; set; }

        /// <summary>
        ///     Gets or sets the full definition.
        /// </summary>
        public string FullDefinition { get; set; }

        /// <summary>
        ///     Gets or sets the short definition.
        /// </summary>
        public string ShortDefinition { get; set; }
        #endregion
    }

    /// <summary>
    ///     The example
    /// </summary>
    [Serializable]
    public class Example
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the media file path.
        /// </summary>
        public string MediaFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
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