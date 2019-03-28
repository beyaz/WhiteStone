namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoInput:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"this.snaps.{SnapName}.getInstance().getValue()";    
        }
    }
}