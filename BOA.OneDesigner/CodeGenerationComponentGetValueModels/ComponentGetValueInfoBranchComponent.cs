namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoBranchComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue().value";    
        }
    }
}