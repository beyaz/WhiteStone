namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoAccountComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().accountNumber";    
        }

        public bool? BindPropertyTypeIsNonNullableNumber { get; set; }
        public bool? BindPropertyTypeIsNullableNumber { get; set; }
    }
}