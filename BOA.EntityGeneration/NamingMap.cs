using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration
{
    public class NamingMap
    {
        #region Fields
        readonly Dictionary<string, string> map = new Dictionary<string, string>();
        #endregion

        #region Public Methods
        public void Push(string key, string value)
        {
            map.AddOrUpdate(key, value);
        }

        public string Resolve(string value)
        {
            var compiledDictionary = ConfigurationDictionaryCompiler.Compile(new Dictionary<string, string> {{nameof(value), value}}, map);

            return compiledDictionary[nameof(value)];
        }
        #endregion
    }
}