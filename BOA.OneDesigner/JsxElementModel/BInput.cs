using System;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BInput : BField
    {
        // TODO string propertiler için editörden set et
        public string Mask { get; set; }

        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsLarge = true};

        #region Public Properties
        public  string Label => LabelInfo.GetDesignerText();

        public LabelInfo LabelInfo { get; set; } = new LabelInfo();


        public string IsVisibleBindingPath{ get; set; }

        public string IsDisabledBindingPath { get; set; }

        #endregion
    }
}