namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoDataGridSelectedValueChangedBindingValue:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"snaps.{SnapName}.getInstance().getSelectedItems()[0]";    
        }
    }
}