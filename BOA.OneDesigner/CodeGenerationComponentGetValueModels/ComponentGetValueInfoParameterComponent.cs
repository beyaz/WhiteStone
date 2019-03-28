namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoParameterComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"this.snaps.{SnapName}.getInstance().getValue().value";    
        }
    }
}