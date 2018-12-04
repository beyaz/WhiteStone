using System;

namespace BOA.CodeGeneration.JsxElementRender
{
    [Serializable]
    public class BParameterComponent: BField
    {
        public bool   ValueTypeIsInt32 { get; set; }
        public string ParamType        { get; set; }
        
    }
}