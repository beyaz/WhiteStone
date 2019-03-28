namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoAccountComponentSuffix:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue().accountSuffix";    
        }
    }
}