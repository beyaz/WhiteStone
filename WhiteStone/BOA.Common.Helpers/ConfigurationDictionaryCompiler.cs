using System;
using System.Collections.Generic;

namespace BOA.Common.Helpers
{
    public static class ConfigurationDictionaryCompiler
    {
        #region Public Methods
        public static IReadOnlyDictionary<string, string> Compile(IReadOnlyDictionary<string, string> dictionary,Func<string,string,string> getValueFunc)
        {
            var pairs = new List<Pair>();

            foreach (var pair in dictionary)
            {
                var item = new Pair
                {
                    key = pair.Key,
                    value = getValueFunc(pair.Key,pair.Value)
                };
                pairs.Add(item);
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