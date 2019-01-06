using System;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The label information
    /// </summary>
    [Serializable]
    public class LabelInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the designer text.
        /// </summary>
        public string DesignerText { get; set; }

        /// <summary>
        ///     Gets or sets the free text value.
        /// </summary>
        public string FreeTextValue { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is free text.
        /// </summary>
        public bool IsFreeText { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is from messaging.
        /// </summary>
        public bool IsFromMessaging { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is request binding path.
        /// </summary>
        public bool IsRequestBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the messaging value.
        /// </summary>
        public string MessagingValue { get; set; }

        /// <summary>
        ///     Gets or sets the request binding path.
        /// </summary>
        public string RequestBindingPath { get; set; }
        #endregion
    }
}