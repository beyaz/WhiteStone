using System.Windows.Data;

namespace CustomUIMarkupLanguage.Markup
{
    /// <summary>
    ///     The binding information contract
    /// </summary>
    public class BindingInfoContract
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the binding mode.
        /// </summary>
        public BindingMode BindingMode { get; set; }

        /// <summary>
        ///     Gets or sets the converter parameter.
        /// </summary>
        public object ConverterParameter { get; set; }

        /// <summary>
        ///     Gets or sets the full name of the converter type.
        /// </summary>
        public string ConverterTypeFullName { get; set; }

        /// <summary>
        ///     Gets or sets the source path.
        /// </summary>
        public string SourcePath { get; set; }
        #endregion
    }
}