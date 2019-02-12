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

        public LabelInfo LabelTextInfo { get; set; }

        public string LabelText => LabelTextInfo.GetDesignerText();

        public bool IsAllOptionIncluded { get; set; }
        #endregion
    }
}