namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoAccountComponentSuffix:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"this.snaps.{SnapName}.getInstance().getValue().accountSuffix";    
        }
    }
}