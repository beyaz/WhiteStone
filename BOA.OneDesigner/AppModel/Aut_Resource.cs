using System;

namespace BOA.OneDesigner.AppModel
{
    /// <summary>
    ///     The aut resource
    /// </summary>
    [Serializable]
    public class Aut_Resource
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the resource code.
        /// </summary>
        public string ResourceCode { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return ResourceCode;
        }
        #endregion
    }
}