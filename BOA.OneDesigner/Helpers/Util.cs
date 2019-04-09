using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.Helpers
{
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