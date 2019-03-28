namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoComboBox:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"this.snaps.{SnapName}.getInstance().getValue().value";    
        }
    }
}