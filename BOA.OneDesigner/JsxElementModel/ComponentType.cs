using System;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The component type
    /// </summary>
    [Serializable]
    public class ComponentType
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is branch component.
        /// </summary>
        public bool IsBranchComponent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is divider.
        /// </summary>
        public bool IsDivider { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is parameter component.
        /// </summary>
        public bool IsParameterComponent { get; set; }
        #endregion
    }
}