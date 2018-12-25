using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCard : Container
    {

        public void RemoveFromParent()
        {
            Container?.RemoveItem(this);
        }

        public CardContainer Container { get; set; }
        #region Public Properties
        public List<BField> Items { get; set; } = new List<BField>();

        public string       Title  { get; set; }
        #endregion

        #region Public Methods
        public override void InsertItem(int index, BField item)
        {
            item.Container = this;

            if (index > Items.Count)
            {
                Items.Add(item);
                return;
            }

            Items.Insert(index, item);
        }

        public override void RemoveItem(BField field)
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

    [Serializable]
    public abstract class Container
    {
        #region Public Methods
        public abstract void InsertItem(int    index, BField item);
        public abstract void RemoveItem(BField field);
        #endregion
    }


   
}