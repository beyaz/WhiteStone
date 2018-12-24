using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCard
    {
        

        #region Public Properties
        public List<BField> Fields { get; set; }
        public string                Title  { get; set; }
        #endregion
    }
}