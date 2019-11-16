using System.Collections.Generic;

namespace BOA.Common.Helpers
{
    public static class ConfigurationDictionaryCompiler
    {
        #region Public Methods
        public static IDictionary<string, string> Compile(IDictionary<string, string> dictionary)
        {
            var pairs = new List<Pair>();

            foreach (var pair in dictionary)
            {
                pairs.Add(new Pair {key = pair.Key, value = pair.Value});
            }

            foreach (var pair in pairs)
            {
                Apply(pairs, pair.key, pair.value);
            }

            var outputDictionary = new Dictionary<string, string>();

            foreach (var pair in pairs)
            {
                outputDictionary.Add(pair.key, pair.value);
            }

            return outputDictionary;
        }
        #endregion

        #region Methods
        static void Apply(List<Pair> pairs, string key, string value)
        {
            foreach (var pair in pairs)
            {
                if (pair.value == null)
                {
                    continue;
                }

                if (pair.key == key)
                {
                    continue;
                }

                pair.value = pair.value.Replace($"$({key})", value);
            }
        }
        #endregion

        class Pair
        {
            #region Fields
            public string key, value;
            #endregion
        }
    }
}