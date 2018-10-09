using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using WhiteStone.Services;

namespace BOA.LanguageTranslations.EnglishBank
{
    public class WordInfo
    {
        public string RawContent { get; set; }
    }

    class Parser
    {
        #region Static Fields
        static readonly int[] AbsentIndex = {251,788,2973, 2974, 2985, 2986, 2988, 2989, 2990 };
        #endregion

        #region Public Methods
        public static IReadOnlyCollection<WordInfo> Parse()
        {
            var filePath = @"D:\github\WhiteStone\WhiteStone\LanguageTranslations\EnglishBank\DataSource.txt";
            return Parse(File.ReadAllText(filePath).Trim());
        }

        public static IReadOnlyCollection<WordInfo> Parse(string allText)
        {
            var words = new List<WordInfo>();

            var lastIndex = 0;

            for (var i = 1; i <= 3000; i++)
            {
                if (AbsentIndex.Contains(i))
                {
                    continue;
                }

                var number = i + ")";
                var index  = allText.IndexOf(number, StringComparison.Ordinal);
                if (index < 0)
                {
                    throw new ArgumentException(i.ToString());
                }

                var wordInfo = new WordInfo
                {
                    RawContent = allText.Substring(lastIndex, index - lastIndex)
                };

                words.Add(wordInfo);

                File.WriteAllText( "d:\\temp\\"+i+".txt", JsonHelper.Serialize(wordInfo));

                lastIndex = index+ number.Length;


            }

            return words;
        }
        #endregion
    }
}