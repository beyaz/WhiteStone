using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace BOA.LanguageTranslations
{

    class Google
    {
        public static WordInfo TranslateEnglishToTurkish(string word)
        {


            var url = "https://translate.google.com/#en/tr/mean";
            var web = new HtmlWeb();
            var doc = web.Load(url, "10.13.50.100",8080,"beyaztas","hattori_hanzo_41");

            var result = doc.DocumentNode.SelectNodes("//*[@id='result_box']")?.FirstOrDefault();

           

            return new WordInfo
            {
                Explanation      = result?.InnerHtml
            };
        }
    }
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

                var turkishPronanciation = doc.DocumentNode.SelectNodes("//*[@id=\"collapseSimilar\"]/div/ul/div[2]/text()")?.FirstOrDefault()?.InnerHtml;

                var items = new List<string>();
                foreach (var node in doc.DocumentNode.SelectNodes("//*[@id=\"ss-section-left\"]/div[1]/div/div[1]/div[2]/dl/dd/p/q"))
                {
                    items.Add(node.InnerHtml);
                }

                return new WordInfo
                {
                    SampleSentences      = items,
                    TurkishPronanciation = turkishPronanciation?.Trim()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IReadOnlyList<WordInfo> GrabAllWords(IEnumerable<string> words)
        {
            var wordInfos = new List< WordInfo >();

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