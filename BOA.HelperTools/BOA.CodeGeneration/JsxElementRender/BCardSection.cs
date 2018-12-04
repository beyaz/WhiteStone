using System;
using System.Collections.Generic;

namespace BOA.CodeGeneration.JsxElementRender
{
    [Serializable]
    public class BCardSection
    {
        #region Public Properties
        public IReadOnlyList<BCard> Cards { get; set; }
        #endregion
    }
}