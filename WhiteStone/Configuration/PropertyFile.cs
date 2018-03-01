using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WhiteStone.Helpers;

namespace WhiteStone.Configuration
{
    /// <summary>
    ///     Manages a Java property file.
    /// </summary>
    public class PropertyFile : IConfigReader
    {
        Dictionary<string, string> _dictionary;
        List<LineInfo> _linesInfos;

        class LineInfo
        {
            readonly string _line;

            public LineInfo(string line)
            {
                _line = line;
            }

            public string Key { get; private set; }

            public string Value { get; private set; }

            public bool IsValid { get; private set; }

            public void Initialize()
            {
                if (!string.IsNullOrEmpty(_line) &&
                    !_line.StartsWith(";", StringComparison.Ordinal) &&
                    !_line.StartsWith("#", StringComparison.Ordinal) &&
                    !_line.StartsWith("'", StringComparison.Ordinal) &&
                    _line.Contains('='))
                {
                    var index = _line.IndexOf('=');
                    var key = _line.Substring(0, index).Trim();
                    var value = _line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"", StringComparison.Ordinal) && value.EndsWith("\"", StringComparison.Ordinal)) ||
                        (value.StartsWith("'", StringComparison.Ordinal) && value.EndsWith("'", StringComparison.Ordinal)))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    Key = key.Replace("\\", string.Empty);
                    Value = value;
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
            }
        }


        /// <summary>
        ///     Loads properties from existing property file.
        /// </summary>
        public PropertyFile LoadFromFile(string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            _linesInfos = ReadLines(file);

            _dictionary = BuildDictionary(_linesInfos);

            return this;
        }

        static List<LineInfo> ReadLines(string file)
        {
            var list = new List<LineInfo>();

            string text = null;
            foreach (var line in File.ReadAllLines(file))
            {
                if ((text != null) && !line.EndsWith("\\", StringComparison.Ordinal))
                {
                    list.Add(new LineInfo(text + line));
                    text = null;
                    continue;
                }

                if (line.EndsWith("\\", StringComparison.Ordinal))
                {
                    text += line.RemoveFromEnd("\\") + Environment.NewLine;
                    continue;
                }

                list.Add(new LineInfo(line));
            }

            return list;
        }

        static Dictionary<string, string> BuildDictionary(List<LineInfo> lines)
        {
            var dictionary = new Dictionary<string, string>();

            lines.ForEach(x => x.Initialize());

            lines.Where(x => x.IsValid).ForEach(info => { dictionary.Add(info.Key, info.Value); });

            return dictionary;
        }

        /// <summary>
        ///     Gets config value by given key.
        /// </summary>
        public string this[string key]
        {
            get
            {
                string value = null;
                _dictionary.TryGetValue(key, out value);
                return value;
            }
        }

        /// <summary>
        ///     Gets all the keys in settings.
        /// </summary>
        public IList<string> AllKeys
        {
            get { return _dictionary.Keys.ToArray(); }
        }
    }
}