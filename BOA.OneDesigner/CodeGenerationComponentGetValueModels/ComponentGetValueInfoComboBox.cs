using BOA.OneDesigner.CodeGenerationModel;

namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoComboBox:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().value";    
        }

        public BindingPathPropertyInfo BindingPathPropertyInfo { get; set; }
    }
}