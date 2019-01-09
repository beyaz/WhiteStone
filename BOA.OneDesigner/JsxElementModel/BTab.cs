using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BTabBar:BField
    {
        #region Public Properties
        public IReadOnlyList<TabPage> Items { get; set; }
        #endregion
    }

    [Serializable]
    public class TabPage
    {
        #region Public Properties
        public DivAsCardContainer DivAsCardContainer { get; set; }

        public string Title => TitleInfo.GetDesignerText();

        public LabelInfo TitleInfo { get; set; } = new LabelInfo();

        #endregion
    }
}