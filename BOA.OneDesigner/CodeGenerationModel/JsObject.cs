using System.Collections.Generic;

namespace BOA.OneDesigner.CodeGenerationModel
{
    public class JsObject : Dictionary<string, string>
    {
        #region Public Properties
        public bool HasValue => Count > 0;
        #endregion
    }
}