using System.Collections.Generic;

namespace BOA.OneDesigner.CodeGenerationModel
{
    class JsBindingPathCalculatorData
    {
        #region Constructors
        public JsBindingPathCalculatorData()
        {
        }

        public JsBindingPathCalculatorData(WriterContext writerContext, string bindingPathInCSharpInDesigner)
        {
            BindingPathInCSharpInDesigner       = bindingPathInCSharpInDesigner;
        }
        #endregion

        #region Public Properties
        public string       BindingPathInCSharpInDesigner       { get; set; }
        public bool         EvaluateInsStateVersion             { get; set; }
        public List<string> RenderMethodRequestRelatedVariables { get; set; }

        public IReadOnlyList<string> Variables { get; set; }
        #endregion

        #region Properties
        internal string BindingPathInJs        { get; set; }
        internal string BindingPathInJsInState { get; set; }
        public string FullBindingPathInJs { get; set; }
        #endregion
    }
}