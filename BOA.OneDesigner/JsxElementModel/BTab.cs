using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BTabBar : BField
    {
        #region Public Properties
        public List<BTabBarPage> Items    { get; set; } = new List<BTabBarPage>();
        public SizeInfo          SizeInfo { get; set; } = new SizeInfo {IsLarge = true};
        #endregion
    }

    [Serializable]
    public class BTabBarPage
    {
        #region Public Properties
        public DivAsCardContainer DivAsCardContainer { get; set; } = new DivAsCardContainer();
        public bool               IsActiveInDesigner { get; set; }

        public bool ShouldRemove { get; set; }

        public string Title => TitleInfo.GetDesignerText();

        public LabelInfo TitleInfo { get; set; } = new LabelInfo();
        #endregion
    }
}