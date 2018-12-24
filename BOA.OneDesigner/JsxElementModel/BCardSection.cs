using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCardSection:CardContainer
    {
        #region Public Properties
        public List<BCard> Cards { get; set; } = new List<BCard>();
        #endregion


        #region Public Methods
        public override void InsertCard(int index, BCard field)
        {
            field.Container = this;
            Cards.Insert(index, field);
        }

        public override void RemoveCard(BCard field)
        {
            if (field.Container != this)
            {
                throw new ArgumentException();
            }

            Cards?.Remove(field);

            field.Container = null;
        }
        #endregion
    }

    [Serializable]
    public abstract class CardContainer
    {
        #region Public Methods
        public abstract void InsertCard(int   index, BCard card);
        public abstract void RemoveCard(BCard card);
        #endregion
    }
}