using System;
using BOA.Common.Helpers;

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
        ///     Gets or sets a value indicating whether this instance is account component.
        /// </summary>
        public bool IsAccountComponent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is branch component.
        /// </summary>
        public bool IsBranchComponent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is button.
        /// </summary>
        public bool IsButton { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is credit card component.
        /// </summary>
        public bool IsCreditCardComponent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is divider.
        /// </summary>
        public bool IsDivider { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is excel browser.
        /// </summary>
        public bool IsExcelBrowser { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is information text.
        /// </summary>
        public bool IsInformationText { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is input.
        /// </summary>
        public bool IsInput { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is label.
        /// </summary>
        public bool IsLabel { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is parameter component.
        /// </summary>
        public bool IsParameterComponent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is position terminal component.
        /// </summary>
        public bool IsPosTerminalComponent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is position merchant component.
        /// </summary>
        public bool IsPosMerchantComponent { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string GetName()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (Convert.ToBoolean(propertyInfo.GetValue(this)))
                {
                    return propertyInfo.Name.RemoveFromStart("Is");
                }
            }

            return "?";
        }
        #endregion
    }
}