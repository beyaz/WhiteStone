using System.Linq;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.Helpers
{
    static class LabelInfoHelper
    {
        #region Public Methods
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

            if (labelInfo.IsFreeText)
            {
                return labelInfo.FreeTextValue;
            }

            if (labelInfo.IsFromMessaging)
            {
                var propertyInfo = MessagingHelper.MessagingPropertyNames?.FirstOrDefault(x => x.PropertyName == labelInfo.MessagingValue);
                if (propertyInfo != null)
                {
                    return propertyInfo.TR_Description;
                }

                return labelInfo.MessagingValue;
            }

            if (labelInfo.IsRequestBindingPath)
            {
                return labelInfo.RequestBindingPath;
            }

            return "?";
        }
        #endregion
    }
}