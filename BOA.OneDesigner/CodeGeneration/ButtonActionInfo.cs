using BOA.OneDesigner.CodeGenerationModel;

namespace BOA.OneDesigner.CodeGeneration
{
    class ButtonActionInfoFunction
    {
        #region Public Properties
        public string DesignerLocation                                 { get; set; }
        public string OpenFormWithResourceCode                         { get; set; }
        public string OpenFormWithResourceCodeDataParameterBindingPath { get; set; }
        public string OrchestrationMethodName                          { get; set; }

        public bool          OpenFormWithResourceCodeIsInDialogBox   { get; set; }
        public string        OrchestrationMethodOnDialogResponseIsOK { get; set; }
        public WriterContext WriterContext                           { get; set; }
        public string        CssOfDialog                             { get; set; }
        #endregion
    }
}