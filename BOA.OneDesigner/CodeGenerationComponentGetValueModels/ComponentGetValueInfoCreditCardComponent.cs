namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoCreditCardComponent : ComponentGetValueInfo
    {
        #region Public Properties
        public string JsPropertyName { get; set; }
        #endregion

        #region Public Methods
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().{JsPropertyName}";
        }
        #endregion
    }
}