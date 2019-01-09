using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BTabBar:BField
    {
        #region Public Properties
        public List<BTabBarPage> Items { get; set; } = new List<BTabBarPage>();
        #endregion
    }

    [Serializable]
    public class BTabBarPage
    {
        public bool ShouldRemove { get; set; }

        #region Public Properties
        public DivAsCardContainer DivAsCardContainer { get; set; } = new DivAsCardContainer();

        public string Title => TitleInfo.GetDesignerText();

        public LabelInfo TitleInfo { get; set; } = new LabelInfo();

        #endregion
    }
}