using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BParameterComponent: BField
    {
        public bool   ValueTypeIsInt32 { get; set; }
        public string ParamType        { get; set; }
        
    }
}