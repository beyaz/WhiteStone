namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoDataGridSelectedValueChangedBindingValue : ComponentGetValueInfo
    {
        #region Public Properties
        public bool IsCollection { get; set; }
        #endregion

        #region Public Methods
        public override string GetCode()
        {
            if (IsCollection)
            {
                return $"snaps.{SnapName}.getInstance().getSelectedItems()";    
            }
            return $"snaps.{SnapName}.getInstance().getSelectedItems()[0]";
        }
        #endregion
    }

    public class ComponentGetValueInfoDataGridSelectedValueChangedBindingValueInBrowseForm : ComponentGetValueInfo
    {
        #region Public Properties
        public bool IsCollection { get; set; }
        #endregion

        #region Public Methods
        public override string GetAssignmentValueCode()
        {
            return GetCode();
        }

        public override string GetCode()
        {
            if (IsCollection)
            {
                return "this.getSelectedRows()";  
            }

            return "this.getSelectedRows()[0]";
        }
        #endregion
    }
}