namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoBranchComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().value;";    
        }
    }
}