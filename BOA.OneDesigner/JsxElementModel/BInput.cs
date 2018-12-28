using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BInput : BField
    {
        public LabelInfo LabelInfo { get; set; } = new LabelInfo();

        public new string Label
        {
            get
            {
                if (LabelInfo?.IsFreeText == true)
                {
                    return LabelInfo.FreeTextValue;
                }

                if (LabelInfo?.IsFromMessaging == true)
                {
                    return LabelInfo.MessagingValue;
                }

                
                if (LabelInfo?.IsRequestBindingPath == true)
                {
                    return LabelInfo.RequestBindingPath;
                }

                return base.Label;
            }
        }
    }
}