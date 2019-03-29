using System.Collections.Generic;

namespace BOA.OneDesigner.CodeGenerationModel
{
    public class JsBindingPathInfo
    {
        #region Constructors
        public JsBindingPathInfo()
        {
        }

        public JsBindingPathInfo(string bindingPathInCSharpInDesigner)
        {
            BindingPathInCSharpInDesigner = bindingPathInCSharpInDesigner;
        }
        #endregion

        #region Public Properties
        public string       BindingPathInCSharpInDesigner       { get; set; }
        public bool         EvaluateInsStateVersion             { get; set; }
        public string       FullBindingPathInJs                 { get; set; }

        public IReadOnlyList<string> Variables { get; set; }
        #endregion

        #region Properties
        internal string BindingPathInJs        { get; set; }
        internal string BindingPathInJsInState { get; set; }
        #endregion
    }
}