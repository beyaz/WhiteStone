using System;

namespace WhiteStone.UI.Container.Mvc
{
    /// <summary>
    ///     The model base
    /// </summary>
    [Serializable]
    public class ModelBase
    {
        #region Public Properties
        /// <summary>
        ///     The focus to component.
        /// </summary>
        public string FocusToComponent { get; set; }

        /// <summary>
        ///     Gets or sets the view message.
        /// </summary>
        public string ViewMessage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [view message type is error].
        /// </summary>
        public bool ViewMessageTypeIsError { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [view should be close].
        /// </summary>
        public bool ViewShouldBeClose { get; set; }
        #endregion
    }
}