using System;
using System.Collections.Generic;
using System.Linq;
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
            var wordInfo = new WordInfo {Word = englishWord};
            Load(wordInfo);
            return wordInfo;
        }

        /// <summary>
        ///     Loads the specified word information.
        /// </summary>
        public static void Load(WordInfo wordInfo)
        {
            if (wordInfo.Word.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(nameof(wordInfo.Word));
            }

            var url = "https://www.ldoceonline.com/dictionary/" + wordInfo.Word;

            var htmlDocument = Helper.GetHtmlDocument(url);

            wordInfo.Dictentries = htmlDocument.DocumentNode.GetElementByClass("dictentry").Select(ParseDictentry).ToList();
            if (wordInfo.SkipInitializeTurkish == false)
            {
                InitializeTurkish(wordInfo);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Initializes the turkish.
        /// </summary>
        static void InitializeTurkish(WordInfo info)
        {
            foreach (var entry in info.Dictentries)
            {
                var topics = entry.Topics;

                for (var i = 0; i < topics.Count; i++)
                {
                    topics[i] += " - " + Google.Translator.TranslateEnglishToTurkish(topics[i]);
                }

                foreach (var usage in entry.Usages)
                {
                    if (usage.FullDefinition.HasValue())
                    {
                        usage.FullDefinitionTR = Google.Translator.TranslateEnglishToTurkish(usage.FullDefinition);
                    }

                    if (usage.ShortDefinition.HasValue())
                    {
                        usage.ShortDefinitionTR = Google.Translator.TranslateEnglishToTurkish(usage.ShortDefinition);
                    }

                    foreach (var example in usage.Examples)
                    {
                        example.TextTR = Google.Translator.TranslateEnglishToTurkish(example.Text);
                    }
                }
            }
        }

        /// <summary>
        ///     Parses the dictentry.
        /// </summary>
        static Entry ParseDictentry(HtmlNode dictentry)
        {
            var result = new Entry
            {
                Topics = dictentry.GetElementByClass("topic").Select(e => e.InnerHtml).ToList(),
                Usages = dictentry.GetElementByClass("Sense").Select(ParseUsageInfo).ToList()
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
                ShortDefinition = newline_Sense.GetElementByClass("SIGNPOST").FirstOrDefault()?.InnerHtml,
                FullDefinition  = newline_Sense.GetElementByClass("DEF").FirstOrDefault()?.InnerText,
                Examples        = newline_Sense.GetElementByClass("EXAMPLE").Select(ParseExample).ToList()
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
        ///     Gets or sets the full definition.
        /// </summary>
        public string FullDefinitionTR { get; set; }

        /// <summary>
        ///     Gets or sets the short definition.
        /// </summary>
        public string ShortDefinition { get; set; }

        /// <summary>
        ///     Gets or sets the short definition.
        /// </summary>
        public string ShortDefinitionTR { get; set; }
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

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        public string TextTR { get; set; }
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

        /// <summary>
        ///     Gets or sets a value indicating whether [skip initialize turkish].
        /// </summary>
        public bool SkipInitializeTurkish { get; set; }

        /// <summary>
        ///     Gets or sets the word.
        /// </summary>
        public string Word { get; set; }
        #endregion
    }
}