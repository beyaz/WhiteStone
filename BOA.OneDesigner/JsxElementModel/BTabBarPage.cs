using System;
using BOA.OneDesigner.Helpers;
using WhiteStone.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The b tab bar page
    /// </summary>
    [Serializable]
    public class BTabBarPage
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the div as card container.
        /// </summary>
        public DivAsCardContainer DivAsCardContainer { get; set; } = new DivAsCardContainer();

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active in designer.
        /// </summary>
        public bool IsActiveInDesigner { get; set; }

        /// <summary>
        ///     Gets the title.
        /// </summary>
        public string Title => TitleInfo.GetDesignerText();

        public string ClassName => "PageModule_"+Title.Replace(" ", "_").Replace("/", "_").Replace("-", "_").Replace("\\", "_").ToString(GlobalizationUtility.EnglishCulture);
        public string ComposedName => "Tab_"+Title.Replace(" ", "_").Replace("/", "_").Replace("-", "_").Replace("\\", "_").ToString(GlobalizationUtility.EnglishCulture);

        /// <summary>
        ///     Gets or sets the title information.
        /// </summary>
        public LabelInfo TitleInfo { get; set; } = new LabelInfo();
        #endregion
    }
}