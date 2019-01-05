using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public abstract class CardContainer
    {
        #region Public Methods
        public abstract void InsertItem(int   index, BCard item);
        public abstract void RemoveItem(BCard card);
        #endregion
    }
}