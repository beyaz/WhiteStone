using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public abstract class Container
    {
        #region Public Methods
        public abstract void InsertItem(int    index, BField item);
        public abstract void RemoveItem(BField field);
        #endregion
    }
}