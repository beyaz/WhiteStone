using System;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BInput : BField
    {
        public SizeInfo SizeInfo { get; set; } = new SizeInfo();

        #region Public Properties
        public new string Label => LabelInfo.GetDesignerText();

        public LabelInfo LabelInfo { get; set; } = new LabelInfo();


        public string IsVisibleBindingPath{ get; set; }

        public string IsDisabledBindingPath { get; set; }

        #endregion
    }
}