using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The b tab bar
    /// </summary>
    [Serializable]
    public class BTabBar : BField
    {
        public string ActiveTabIndexBindingPath { get; set; }

        #region Public Properties
        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        public List<BTabBarPage> Items { get; set; } = new List<BTabBarPage>();

        /// <summary>
        ///     Gets or sets the size information.
        /// </summary>
        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsLarge = true};

        #endregion
    }
}