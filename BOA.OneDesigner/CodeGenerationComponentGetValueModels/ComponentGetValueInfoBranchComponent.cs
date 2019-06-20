namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoBranchComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().branch.branchId";    
        }
    }
}