using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class Component : BField
    {
        #region Public Properties
        public ComponentType Type { get; set; }
        #endregion
    }
}