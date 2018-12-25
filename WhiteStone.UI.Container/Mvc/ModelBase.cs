using System;
using System.Collections.Generic;

namespace WhiteStone.UI.Container.Mvc
{
    /// <summary>
    ///     The action button information
    /// </summary>
    [Serializable]
    public class ActionButtonInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
        #endregion
    }

    /// <summary>
    ///     The model base
    /// </summary>
    [Serializable]
    public class ModelBase
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the action buttons.
        /// </summary>
        public List<ActionButtonInfo> ActionButtons { get; set; }

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