using System;
using System.Linq;
using BOA.OneDesigner.WpfControls;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BInput : BField
    {
        #region Public Properties
        public new string Label => LabelInfo.GetDesignerText();

        public LabelInfo LabelInfo { get; set; } = new LabelInfo();
        #endregion
    }

    static class LabelInfoHelper
    {
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
                var propertyInfo = UIContext.MessagingPropertyNames?.FirstOrDefault(x => x.PropertyName == labelInfo.MessagingValue);
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