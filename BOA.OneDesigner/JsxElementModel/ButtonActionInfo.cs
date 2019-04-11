namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The button action information
    /// </summary>
    class ButtonActionInfo
    {
        public string YesNoQuestion { get; set; }

        public string YesNoQuestionAfterYesOrchestrationCall { get; set; }

        #region Public Properties
        /// <summary>
        ///     Gets or sets the CSS of dialog.
        /// </summary>
        public string CssOfDialog { get; set; }

        /// <summary>
        ///     Gets or sets the designer location.
        /// </summary>
        public string DesignerLocation { get; set; }

        /// <summary>
        ///     Gets or sets the name of the extension method.
        /// </summary>
        public string ExtensionMethodName { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code.
        /// </summary>
        public string OpenFormWithResourceCode { get; set; }

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

        public string Title { get; set; }
        #endregion
    }
}