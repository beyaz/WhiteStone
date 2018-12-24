using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCardSection
    {
        #region Public Properties
        public IReadOnlyList<BCard> Cards { get; set; }
        #endregion
    }
}