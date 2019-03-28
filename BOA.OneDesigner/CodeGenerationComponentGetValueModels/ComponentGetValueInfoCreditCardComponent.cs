namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoCreditCardComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().clearCardNumber;";    
        }
    }
}