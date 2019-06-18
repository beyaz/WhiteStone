namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    /// <summary>
    ///     The component get value information
    /// </summary>
    public abstract class ComponentGetValueInfo
    {
        public const string VariableNameOfComponent = "cmp";

        #region Public Properties
        /// <summary>
        ///     Gets or sets the js binding path.
        /// </summary>
        public string JsBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the name of the snap.
        /// </summary>
        public string SnapName { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the code.
        /// </summary>
        public abstract string GetCode();


        public virtual string GetAssignmentValueCode()
        {
            return $"{GetCode()}";
        }
        #endregion
    }
}