namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoDataGridSelectedValueChangedBindingValue:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getSelectedItems()[0]";    
        }
    }

    public class ComponentGetValueInfoDataGridSelectedValueChangedBindingValueInBrowseForm:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return "this.getSelectedRows()[0]"; 
        }

        public override string GetAssignmentValueCode()
        {
            return GetCode();
        }
    }

    
}