using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The action information
    /// </summary>
    [Serializable]
    public class ActionInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the CSS of dialog.
        /// </summary>
        public string CssOfDialog { get; set; }

        /// <summary>
        ///     Gets or sets the dialog title.
        /// </summary>
        public string DialogTitle { get; set; }

      
        public LabelInfo DialogTitleInfo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the extension method.
        /// </summary>
        public string ExtensionMethodName { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code.
        /// </summary>
        public string OpenFormWithResourceCode { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code condition.
        /// </summary>
        public string OpenFormWithResourceCodeCondition { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code data parameter binding path.
        /// </summary>
        public string OpenFormWithResourceCodeDataParameterBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [open form with resource code is in dialog box].
        /// </summary>
        public bool OpenFormWithResourceCodeIsInDialogBox { get; set; }

        /// <summary>
        ///     Gets or sets the name of the orchestration method.
        /// </summary>
        public string OrchestrationMethodName { get; set; }

        /// <summary>
        ///     Gets or sets the orchestration method on dialog response is ok.
        /// </summary>
        public string OrchestrationMethodOnDialogResponseIsOK { get; set; }

        /// <summary>
        ///     Gets or sets the yes no question.
        /// </summary>
        public string YesNoQuestion { get; set; }

        public LabelInfo YesNoQuestionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the yes no question after yes orchestration call.
        /// </summary>
        public string YesNoQuestionAfterYesOrchestrationCall { get; set; }

        /// <summary>
        ///     Gets or sets the yes no question condition.
        /// </summary>
        public string YesNoQuestionCondition { get; set; }
        #endregion

        public bool HasValue()
        {

            if (OrchestrationMethodName.HasValue() || 
                YesNoQuestionInfo.HasValue())
            {
                return true;
            }

            if (OpenFormWithResourceCode.HasValue())
            {
                return true;
            }
            if (ExtensionMethodName.HasValue())
            {
                return true;
            }

            return false;
        }
    }
}