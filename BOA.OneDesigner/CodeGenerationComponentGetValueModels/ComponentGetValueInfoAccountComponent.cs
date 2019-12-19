namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoAccountComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            if (BindPropertyTypeIsNonNullableNumber == true)
            {
                return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().accountNumber|0"; 
            }
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().accountNumber";    
        }

        public bool? BindPropertyTypeIsNonNullableNumber { get; set; }
        public bool? BindPropertyTypeIsNullableNumber { get; set; }
    }
}