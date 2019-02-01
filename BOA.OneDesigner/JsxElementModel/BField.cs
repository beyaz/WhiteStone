using System;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The b field
    /// </summary>
    [Serializable]
    public abstract class BField
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the container.
        /// </summary>
        public Container Container { get; set; }

        /// <summary>
        ///     Gets or sets the name of the snap.
        /// </summary>
        public string SnapName { get; set; }

        /// <summary>
        ///     Gets or sets the value binding path.
        /// </summary>
        public string ValueBindingPath { get; set; }

        /// <summary>
        ///     Gets the value binding path in type script.
        /// </summary>
        public virtual string ValueBindingPathInTypeScript => TypescriptNaming.NormalizeBindingPath(Config.Value + ValueBindingPath);
        #endregion

        #region Public Methods
        /// <summary>
        ///     Removes from parent.
        /// </summary>
        public void RemoveFromParent()
        {
            Container?.RemoveItem(this);
        }
        #endregion
    }
}