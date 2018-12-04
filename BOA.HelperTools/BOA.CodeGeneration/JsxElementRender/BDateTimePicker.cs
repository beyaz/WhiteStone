using System;

namespace BOA.CodeGeneration.JsxElementRender
{
    [Serializable]
    public class BDateTimePicker : BField
    {
        
    }

    [Serializable]
    public class BAccountComponent : BField
    {
       
    }

    [Serializable]
    public class BInput: BField
    {
        
    }
    
    [Serializable]
    public class BInputNumeric: BField
    {
        public bool IsDecimal { get; set; }
        public bool IsNumber { get; set; }

        public int MaxLength => IsDecimal ? 22 : 10;

       
    }

    
    [Serializable]
    public class BCheckBox: BField
    {
       
    }
    
    [Serializable]
    public class BBranchComponent: BField
    {
        
    }

    [Serializable]
    public class BParameterComponent: BField
    {
        public bool ValueTypeIsInt32 { get; set; }
        public string ParamType { get; set; }
        
    }
    
    
    
}