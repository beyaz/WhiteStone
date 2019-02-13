using System;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The component information
    /// </summary>
    [Serializable]
    public class ComponentInfo : BField
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the information text.
        /// </summary>
        public LabelInfo InfoText { get; set; }

        /// <summary>
        ///     Gets the information text value.
        /// </summary>
        public string InfoTextValue => InfoText.GetDesignerText();

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is all option included.
        /// </summary>
        public bool IsAllOptionIncluded { get; set; }

        /// <summary>
        ///     Gets or sets the is disabled binding path.
        /// </summary>
        public string IsDisabledBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the is visible binding path.
        /// </summary>
        public string IsVisibleBindingPath { get; set; }

        /// <summary>
        ///     Gets the label text.
        /// </summary>
        public string LabelText => LabelTextInfo.GetDesignerText();

        /// <summary>
        ///     Gets or sets the label text information.
        /// </summary>
        public LabelInfo LabelTextInfo { get; set; }

        /// <summary>
        ///     Gets or sets the type of the parameter.
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        ///     Gets or sets the size information.
        /// </summary>
        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsLarge = true};

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public ComponentType Type { get; set; }
        #endregion
    }
}