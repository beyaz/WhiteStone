using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class DivAsCardContainer:CardContainer
    {
        #region Public Properties
        public List<BCard> Items { get; set; } = new List<BCard>();
        #endregion


        #region Public Methods
        public override void InsertItem(int index, BCard item)
        {
            item.Container = this;
            if (index > Items.Count)
            {
                Items.Add(item);
                return;
            }
            Items.Insert(index, item);
        }

        public override void RemoveItem(BCard field)
        {
            if (field.Container != this)
            {
                throw new ArgumentException();
            }

            Items?.Remove(field);

            field.Container = null;
        }
        #endregion
    }
}