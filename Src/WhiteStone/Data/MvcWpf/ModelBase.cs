using System;
using WhiteStone.Common;

namespace BOA.Data.MvcWpf
{
    /// <summary>
    ///     Base model definition.
    /// </summary>
    [Serializable]
    public class ModelBase : ContractBase, IModel
    {
        enum ViewMessageTypes
        {
            Info,
            Warning
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelBase" /> class.
        /// </summary>
        public ModelBase()
        {
            ViewMessageType = ViewMessageTypes.Info.ToString();
        }

        /// <summary>
        ///     Gets or sets the view message.
        /// </summary>
        public string ViewMessage { get; set; }

        /// <summary>
        ///     Gets or sets the type of the view message.
        /// </summary>
        public string ViewMessageType { get; set; }

        /// <summary>
        ///     The warning message.
        /// </summary>
        public string WarningMessage
        {
            get { return ViewMessage; }
            set
            {
                ViewMessage = value;

                if (value != null)
                {
                    ViewMessageType = ViewMessageTypes.Warning.ToString();
                }
                else
                {
                    ViewMessageType = ViewMessageTypes.Info.ToString();
                }
            }
        }

        /// <summary>
        ///     The focus to component.
        /// </summary>
        public string FocusToComponent { get; set; }

        /// <summary>
        ///     Gets or sets the comeback when these properties changed.
        /// </summary>
        public string ComebackWhenThesePropertiesChanged { get; set; }

        /// <summary>
        ///     The view command.
        ///     <para>will be invoke method names in view </para>
        /// </summary>
        public string ViewCommand { get; set; }

        /// <summary>
        ///     Gets or sets the call back method name for calling controller
        /// </summary>
        public string Callback { get; set; }
    }
}