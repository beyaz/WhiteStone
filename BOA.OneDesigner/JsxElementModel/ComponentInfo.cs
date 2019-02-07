using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class ComponentInfo : BField
    {
        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsLarge = true};

        #region Public Properties
        public ComponentType Type { get; set; }
        #endregion
    }
}