using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BBranchComponent: BField
    {
        public LabelInfo LabelInfo { get; set; } = new LabelInfo();
    }
}