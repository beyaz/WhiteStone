using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCard : Container
    {
        #region Public Properties
        public CardContainer Container { get; set; }
        public List<BField>  Items     { get; set; } = new List<BField>();

        public LayoutProps LayoutProps { get; set; } = new LayoutProps {Wide = 5};

        public string Title => TitleInfo.GetDesignerText();

        public LabelInfo TitleInfo { get; set; } = new LabelInfo();
        public bool ShouldBeDelete { get; set; }
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

        public void RemoveFromParent()
        {
            Container?.RemoveItem(this);
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
}