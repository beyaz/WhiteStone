using System.IO;
using System.Linq;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.Helpers
{
    static class FileUtil
    {
        #region Public Methods
        public static string GetTfsPath(string filePath)
        {
            return "$" + filePath.RemoveFromStart("d:\\work").Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static bool TryWrite(string path, string oldContent, string newContent)
        {
            var isEqual = StringHelper.IsEqualAsData(oldContent, newContent);
            if (isEqual)
            {
                return false;
            }

            if (!File.Exists(path))
            {
                File.WriteAllText(path, newContent);
                return true;
            }

            var errorMessage = TFSAccessForBOA.CheckoutFile(path);
            if (errorMessage.HasValue())
            {
                Log.Push("Checkout is failed. @errorMessage:" + errorMessage);
            }

            // File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);

            File.WriteAllText(path, newContent);

            return true;
        }
        #endregion
    }

    static class Utilization
    {
        #region Public Methods
        public static bool HasExtensionFile(ScreenInfo screenInfo)
        {
            var hasAnyResourceActionContainsCustomFunction = screenInfo.ResourceActions?.Any(x => x.ExtensionMethodName.HasValue()) == true;

            return screenInfo.ExtensionAfterConstructor || screenInfo.ExtensionAfterProxyDidRespond || hasAnyResourceActionContainsCustomFunction;
        }
        #endregion
    }
}