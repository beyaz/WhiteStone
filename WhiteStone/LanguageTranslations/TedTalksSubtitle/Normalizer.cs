using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BOA.LanguageTranslations.TedTalksSubtitle
{
    /// <summary>
    ///     The normalizer
    /// </summary>
    public static class Normalizer
    {
        #region Static Fields
        static readonly string[] TrLineFinishers =
        {
            ".",
            "?",
            ".(Kahkahalar)"
        };
        #endregion

        #region Public Methods
        /// <summary>
        ///     Normalizes the specified content.
        /// </summary>
        public static string Normalize(string content)
        {
            var lines = new List<LineInfo>();

            string line       = null;
            string nextLine   = null;
            var    lineNumber = 0;

            var strings = content.Split(Environment.NewLine.ToCharArray()).ToArray();
            var length  = strings.Length;
            for (var i = 0; i < length; i++)
            {
                line = strings[i];

                nextLine = null;
                if (i + 1 < length)
                {
                    nextLine = strings[i + 1];
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (int.TryParse(line, out lineNumber))
                {
                    if (nextLine?.Contains(" --> ") == true)
                    {
                        lines.Add(new LineInfo
                        {
                            Tr = strings[i + 2],
                            En = strings[i + 3]
                        });
                        i = i + 3;
                    }
                }
            }

            // start to match line for sentences

            lines = CombineForSentences(lines);

            var sb = new StringBuilder();
            foreach (var lineInfo in lines)
            {
                sb.AppendLine("");
                sb.AppendLine(lineInfo.Tr.Trim());
                sb.AppendLine(lineInfo.En.Trim());
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        ///     Normalizes the file.
        /// </summary>
        public static void NormalizeFile(string path)
        {
            var content = Normalize(File.ReadAllText(path));

            File.WriteAllText(path + ".simplified.txt", content);
        }
        #endregion

        #region Methods
        static List<LineInfo> CombineForSentences(List<LineInfo> lines)
        {
            var sentences = new List<LineInfo>();

            var index = 0;

            for (var i = 0; i < lines.Count; i++)
            {
                var line   = lines[i];
                var trLine = line.Tr.Trim();

                if (TrLineFinishers.Any(x => trLine.EndsWith(x)))
                {
                    var tr = "";
                    var en = "";
                    for (; index <= i; index++)
                    {
                        tr += " " + lines[index].Tr;
                        en += " " + lines[index].En;
                    }

                    sentences.Add(new LineInfo
                    {
                        Tr = tr,
                        En = en
                    });
                }
            }

            return sentences;
        }
        #endregion

        class LineInfo
        {
            #region Public Properties
            public string En { get; set; }
            public string Tr { get; set; }
            #endregion
        }
    }
}