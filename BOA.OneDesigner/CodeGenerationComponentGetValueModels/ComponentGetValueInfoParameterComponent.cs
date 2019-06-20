namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoParameterComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().paramCode";    
        }
    }
}