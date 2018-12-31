using System;
using BOA.OneDesigner.Helpers;
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
}