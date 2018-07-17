using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BOA.Common.Helpers;
using HtmlAgilityPack;

namespace BOA.LanguageTranslations.Longman
{
    /// <summary>
    ///     The translator
    /// </summary>
    public class Translator
    {
        #region Public Methods
        /// <summary>
        ///     Gets the word information.
        /// </summary>
        public static WordInfo GetWordInfo(string englishWord)
        {
            if (englishWord.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(nameof(englishWord));
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var url = "https://www.ldoceonline.com/dictionary/" + englishWord;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            return InitializeTurkish(new WordInfo
            {
                Dictentries = doc.DocumentNode.GetElementbyClass("dictentry").Select(ParseDictentry).ToList()
            });
        }

        static WordInfo InitializeTurkish(WordInfo info)
        {
            foreach (var entry in info.Dictentries)
            {
                var topics = entry.Topics;

                for (var i = 0; i < topics.Count; i++)
                {
                    topics[i] += " - "+ Google.EnglishToTurkishTranslator.Translate(topics[i]);
                }
            }

            return info;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Parses the dictentry.
        /// </summary>
        static Entry ParseDictentry(HtmlNode dictentry)
        {
            var result = new Entry
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
    ///     The entry
    /// </summary>
    [Serializable]
    public class Entry
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
    public class WordInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the dictentries.
        /// </summary>
        public List<Entry> Dictentries { get; set; }
        #endregion
    }
}