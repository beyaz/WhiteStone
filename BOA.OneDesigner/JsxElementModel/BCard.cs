using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The b card
    /// </summary>
    [Serializable]
    public class BCard : Container
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the container.
        /// </summary>
        public CardContainer Container { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is browse page criteria.
        /// </summary>
        public bool IsBrowsePageCriteria { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is browse page data grid container.
        /// </summary>
        public bool IsBrowsePageDataGridContainer { get; set; }

        /// <summary>
        ///     Gets or sets the is visible binding path.
        /// </summary>
        public string IsVisibleBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        public List<BField> Items { get; set; } = new List<BField>();

        /// <summary>
        ///     Gets or sets the layout props.
        /// </summary>
        public LayoutProps LayoutProps { get; set; } = new LayoutProps {Wide = 5};

        /// <summary>
        ///     Gets the title.
        /// </summary>
        public string Title => TitleInfo.GetDesignerText();

        /// <summary>
        ///     Gets or sets the title information.
        /// </summary>
        public LabelInfo TitleInfo { get; set; } = new LabelInfo();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Inserts the item.
        /// </summary>
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

        /// <summary>
        ///     Removes from parent.
        /// </summary>
        public void RemoveFromParent()
        {
            Container?.RemoveItem(this);
        }

        /// <summary>
        ///     Removes the item.
        /// </summary>
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