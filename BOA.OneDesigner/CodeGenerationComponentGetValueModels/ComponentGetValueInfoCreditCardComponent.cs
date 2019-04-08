namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoCreditCardComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue().clearCardNumber";    
        }
    }

    public class ComponentGetValueInfoCreditCardComponentCardRefNumber:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getValue().cardRefNumber";    
        }
    }
}