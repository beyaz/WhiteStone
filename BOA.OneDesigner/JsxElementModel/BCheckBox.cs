using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCheckBox: BField
    {
        public LabelInfo LabelInfo { get; set; } = new LabelInfo();
       
    }
}