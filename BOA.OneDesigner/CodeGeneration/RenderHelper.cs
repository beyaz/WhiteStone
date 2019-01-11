using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class RenderHelper
    {
        public static string GetJsValue(SizeInfo size)
        {
			
			
            if (size.IsLarge)
            {
                return "ComponentSize.LARGE";
            }
            if (size.IsMedium)
            {
                return "ComponentSize.MEDIUM";
            }
            if(size.IsSmall)
            {
                return "ComponentSize.SMALL";
            }
            if(size.IsExtraSmall)
            {
                return "ComponentSize.XSMALL";
            }

            throw Error.InvalidOperation();
        }

        public static string GetLabelValue(ScreenInfo screenInfo, LabelInfo data)
        {
            if (data == null)
            {
                return null;
            }

            if (data.IsFreeText)
            {
                return '"' + data.FreeTextValue + '"';
            }

            if (data.IsRequestBindingPath)
            {
                return TypescriptNaming.NormalizeBindingPath("request." + data.RequestBindingPath);
            }

            if (data.IsFromMessaging)
            {
                return $"getMessage(\"{screenInfo.MessagingGroupName}\", \"{data.MessagingValue}\")";
            }

            throw Error.InvalidOperation();
        }
    }
}