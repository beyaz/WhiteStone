namespace WhiteStone.Data.MvcWpf
{
    /// <summary>
    ///     Defines the i model.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        ///     Gets or sets the view message.
        /// </summary>
        string ViewMessage { get; set; }

        /// <summary>
        ///     Gets or sets the type of the view message.
        /// </summary>
        string ViewMessageType { get; set; }

        /// <summary>
        ///     The focus to component.
        /// </summary>
        string FocusToComponent { get; set; }

        /// <summary>
        ///     The view command.
        ///     <para>will be invoke method names in view </para>
        /// </summary>
        string ViewCommand { get; set; }

        /// <summary>
        ///     Gets or sets the call back method name for calling controller
        /// </summary>
        string Callback { get; set; }

        /// <summary>
        ///     The warning message.
        /// </summary>
        string WarningMessage { get; set; }

        /// <summary>
        ///     Gets or sets the comeback when these properties changed.
        /// </summary>
        string ComebackWhenThesePropertiesChanged { get; set; }
    }
}