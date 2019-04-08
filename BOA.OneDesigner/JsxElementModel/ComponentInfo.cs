using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The component information
    /// </summary>
    [Serializable]
    public class ComponentInfo : BField
    {

        public string CreditCardDesignerText => ValueBindingPath.HasValue() ? ValueBindingPath : CardRefNumberBindingPath;

        public string CardRefNumberBindingPath { get; set; }

        public string AccountNumberBindingPath { get; set; }


        


        public string AccountSuffixBindingPath { get; set; }

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
        ///     Gets or sets a value indicating whether this instance is bold.
        /// </summary>
        public bool IsBold { get; set; }

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
        ///     Gets or sets the mask.
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        ///     Gets or sets the type of the parameter.
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        ///     Gets or sets the row count.
        /// </summary>
        public int? RowCount { get; set; }

        public int? MaxLength { get; set; }
        

        /// <summary>
        ///     Gets or sets the size information.
        /// </summary>
        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsLarge = true};

        /// <summary>
        ///     Gets the text.
        /// </summary>
        public string Text => TextInto.GetDesignerText();

        /// <summary>
        ///     Gets or sets the text into.
        /// </summary>
        public LabelInfo TextInto { get; set; } = new LabelInfo {IsFreeText = true, FreeTextValue = "Label"};

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public ComponentType Type { get; set; }

        /// <summary>
        ///     Gets or sets the value changed orchestration method.
        /// </summary>
        public string ValueChangedOrchestrationMethod { get; set; }


        public string ButtonClickedOrchestrationMethod { get; set; }


        /// <summary>
        ///     Gets or sets the open form with resource code.
        /// </summary>
        public string OpenFormWithResourceCode { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code data parameter binding path.
        /// </summary>
        public string OpenFormWithResourceCodeDataParameterBindingPath { get; set; }

        public bool OpenFormWithResourceCodeIsInDialogBox { get; set; }


        public string OrchestrationMethodOnDialogResponseIsOK { get; set; }

        public string CssOfDialog { get; set; }


        public string ExtensionMethodName { get; set; }

        #endregion
    }
}