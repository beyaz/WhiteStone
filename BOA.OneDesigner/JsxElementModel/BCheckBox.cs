using System;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.WpfControls;

namespace BOA.OneDesigner.JsxElementModel
{
   

    /// <summary>
    ///     The b label
    /// </summary>
    [Serializable]
    public class BLabel : BField, ISupportSizeInfo
    {
        #region Public Properties
        public bool IsBold { get; set; }

        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsLarge = true};

        /// <summary>
        ///     Gets the text.
        /// </summary>
        public string Text => TextInto.GetDesignerText();

        /// <summary>
        ///     Gets or sets the text into.
        /// </summary>
        public LabelInfo TextInto { get; set; } = new LabelInfo {IsFreeText = true, FreeTextValue = "Label"};
        #endregion
    }
}