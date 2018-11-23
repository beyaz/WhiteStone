using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.LanguageTranslations.EnglishBank
{
    /// <summary>
    ///     The word information
    /// </summary>
    [Serializable]
    public class WordInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the details.
        /// </summary>
        public IReadOnlyCollection<string> Details { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        public string Key { get; set; }
        #endregion

        #region Properties
        internal string RawContent { get; set; }
        #endregion

        #region Methods
        internal void Parse()
        {
            var arr = RawContent.Split(";(".ToCharArray());
            Key = arr[0].Trim();

            var lines = new List<string>();
            var i     = 0;
            foreach (var line in RawContent.Split(Environment.NewLine.ToCharArray()).Where(x => string.IsNullOrWhiteSpace(x) == false).Select(x=>x.Trim()))
            {
                if (i++ == 0 && line.StartsWith(Key))
                {
                    if (!string.IsNullOrWhiteSpace(line.RemoveFromStart(Key)))
                    {
                        lines.Add(line.RemoveFromStart(Key).RemoveFromStart(";").Trim());
                    }

                    continue;
                }

                lines.Add(line.Trim().RemoveFromStart(";").Trim());
            }

            Details = lines;
        }
        #endregion
    }

    class Parser
    {
        #region Static Fields
        static readonly int[] AbsentIndex = {251, 788, 2973, 2974, 2985, 2986, 2988, 2989, 2990};
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

                wordInfo.Parse();

                words.Add(wordInfo);

                File.WriteAllText($"d:\\temp\\{i +" - "+ wordInfo.Key}.json", JsonHelper.Serialize(wordInfo));

                lastIndex = index + number.Length;
            }

            return words;
        }
        #endregion
    }
}