namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoAccountComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue().accountNumber";    
        }
        
    }
}