using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementRender
{
    [Serializable]
    public class BCard
    {
        

        #region Public Properties
        public IReadOnlyList<BField> Fields { get; set; }
        public string                Title  { get; set; }
        #endregion
    }
}