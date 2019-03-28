namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoInput:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue();";    
        }
    }
}