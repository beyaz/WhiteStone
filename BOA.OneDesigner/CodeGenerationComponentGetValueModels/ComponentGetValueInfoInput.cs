namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoInput:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue()";    
        }
    }

    public class ComponentGetValueInfoInputMask:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue().saltValue";    
        }
    }


    public class ComponentGetValueInfoExcelBrowser:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue()";    
        }
    }
}