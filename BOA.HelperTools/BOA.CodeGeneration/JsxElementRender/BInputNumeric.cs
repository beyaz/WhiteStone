using System;

namespace BOA.CodeGeneration.JsxElementRender
{
    [Serializable]
    public class BInputNumeric: BField
    {
        public bool IsDecimal { get; set; }
        public bool IsNumber  { get; set; }

        public int MaxLength => IsDecimal ? 22 : 10;

       
    }
}