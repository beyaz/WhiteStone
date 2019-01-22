using System;

namespace BOA.OneDesigner.AppModel
{
    /// <summary>
    ///     The aut resource action
    /// </summary>
    [Serializable]
    public class Aut_ResourceAction
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the command.
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        ///     Gets or sets the is visible binding path.
        /// </summary>
        public string IsVisibleBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code.
        /// </summary>
        public string OpenFormWithResourceCode { get; set; }

        /// <summary>
        ///     Gets or sets the open form with resource code data parameter binding path.
        /// </summary>
        public string OpenFormWithResourceCodeDataParameterBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the name of the orchestration method.
        /// </summary>
        public string OrchestrationMethodName { get; set; }
        #endregion
    }
}