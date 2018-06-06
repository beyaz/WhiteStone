using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace BOA.LanguageTranslations
{
    class SesliSözlük
    {
        #region Public Methods
        public static WordInfo GetWord(string word)
        {
            try
            {
                var url = "https://www.seslisozluk.net/en/what-is-the-meaning-of-" + word + "/";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var turkishPronanciation = doc.DocumentNode.SelectNodes("//*[@id=\"collapseSimilar\"]/div/ul/div[1]/text()")?.FirstOrDefault()?.InnerHtml;
                var wordInfo = new WordInfo
                {
                    Word                 = word,
                    Means                = new List<MeanInfo>(),
                    TurkishPronanciation = turkishPronanciation?.Trim()
                };

                var meanNodes = doc.DocumentNode.SelectNodes("//*[@id=\"ss-section-left\"]/div[1]/div/div[1]/div[2]/dl/dd");

                foreach (var node in meanNodes)
                {
                    var meanInfo = new MeanInfo
                    {
                        Definition      = node.SelectNodes("a")?.FirstOrDefault()?.InnerHtml,
                        SampleSentences = new List<string>()
                    };
                    wordInfo.Means.Add(meanInfo);

                    var sampleNodes = node.SelectNodes("p/q");

                    if (sampleNodes != null)
                    {
                        foreach (var sampleNode in sampleNodes)
                        {
                            meanInfo.SampleSentences.Add(sampleNode.InnerHtml);
                        }
                    }
                }

                return wordInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IReadOnlyList<WordInfo> GrabAllWords(IEnumerable<string> words)
        {
            var wordInfos = new List<WordInfo>();

            foreach (var item in words)
            {
                var word = item.Trim();

                var wordInfo = GetWord(word);
                if (wordInfo == null)
                {
                    continue;
                }

                wordInfos.Add(wordInfo);
            }

            return wordInfos;
        }
        #endregion

        #region Methods
        static void PushToFile(string filePath, object instance)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fileStream))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    var ser = JsonSerializer.Create(new JsonSerializerSettings
                    {
                        Formatting        = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    ser.Serialize(jsonWriter, instance);
                    jsonWriter.Flush();
                }
            }
        }
        #endregion
    }
}