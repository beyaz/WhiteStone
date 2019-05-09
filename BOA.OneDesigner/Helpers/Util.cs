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
            bool hasExtensionMethod = false;
            VisitHelper.VisitComponents(screenInfo, (component) =>
            {
                if (component is BDataGrid  dataGrid)
                {
                    if (dataGrid.RowSelectionChangedActionInfo.ExtensionMethodName.HasValue())
                    {
                        hasExtensionMethod = true;
                    }
                }
            });

            if (hasExtensionMethod)
            {
                return true;
            }


            var hasAnyResourceActionContainsCustomFunction = screenInfo.ResourceActions?.Any(x => x.OnClickAction?.ExtensionMethodName?.HasValue() == true) == true;

            return screenInfo.ExtensionAfterConstructor || screenInfo.ExtensionAfterProxyDidRespond || hasAnyResourceActionContainsCustomFunction;
        }
        #endregion
    }
}