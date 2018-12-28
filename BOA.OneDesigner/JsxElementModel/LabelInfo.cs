using System;
using WhiteStone.Common;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class LabelInfo
    {
        public bool   IsFreeText      { get; set; }
        public bool   IsFromMessaging { get; set; }
        public bool IsRequestBindingPath { get; set; }

        public string FreeTextValue   { get; set; }
        public string MessagingValue  { get; set; }

        public string RequestBindingPath { get; set; }

        public string DesignerText    { get; set; }
    }
}