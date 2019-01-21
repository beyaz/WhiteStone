using System;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The container
    /// </summary>
    [Serializable]
    public abstract class Container
    {
        #region Public Methods
        /// <summary>
        ///     Inserts the item.
        /// </summary>
        public abstract void InsertItem(int index, BField item);

        /// <summary>
        ///     Removes the item.
        /// </summary>
        public abstract void RemoveItem(BField field);
        #endregion
    }
}