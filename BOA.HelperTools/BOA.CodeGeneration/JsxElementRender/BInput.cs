using System;

namespace BOA.CodeGeneration.JsxElementRender
{
    [Serializable]
    public class BInput : BField
    {
        #region Public Methods
        public override string ToString()
        {
            var template = new BInputTemplate {Data = this};

            template.PushIndent(Indent);

            return template.TransformText();
        }
        #endregion
    }
}