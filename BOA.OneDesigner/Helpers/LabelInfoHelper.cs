using System.Linq;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.Helpers
{
    static class LabelInfoHelper
    {

        public static LabelInfo CreateNewLabelInfo(string freeText = null)
        {
            return new LabelInfo
            {
                IsFreeText    = true,
                FreeTextValue = freeText ?? "?",
                DesignerText  = "Label"
            };
        }

        public static string GetDesignerText(this LabelInfo labelInfo)
        {
            if (labelInfo == null)
            {
                return "?";
            }
            if (labelInfo?.IsFreeText == true)
            {
                return labelInfo.FreeTextValue;
            }

            if (labelInfo?.IsFromMessaging == true)
            {
                var propertyInfo = MessagingHelper.MessagingPropertyNames?.FirstOrDefault(x => x.PropertyName == labelInfo.MessagingValue);
                if (propertyInfo != null)
                {
                    return propertyInfo.TR_Description;
                }

                return labelInfo.MessagingValue;
            }

            if (labelInfo?.IsRequestBindingPath == true)
            {
                return labelInfo.RequestBindingPath;
            }

            return "?";

        }
    }
}