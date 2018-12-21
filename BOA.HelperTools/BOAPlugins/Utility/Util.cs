using System.IO;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;

namespace BOAPlugins.Utility
{
    public static class Util
    {
        #region Public Methods
        public static void WriteFileIfContentNotEqual(string path, string content)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, content);
                return;
            }

            var existingData = File.ReadAllText(path);

            var isEqual = StringHelper.IsEqualAsData(existingData, content);

            if (!isEqual)
            {
                TFSAccessForBOA.CheckoutFile(path);
                File.WriteAllText(path, content);
            }
        }
        #endregion
    }
}