using System;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public abstract class BField : BComponent
    {
    }

    [Serializable]
    public class BComponent
    {
        #region Public Properties
        public Container Container { get; set; }

        public string SnapName         { get; set; }
        public string ValueBindingPath { get; set; }

        public virtual string ValueBindingPathInTypeScript => TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + ValueBindingPath);
        #endregion

        #region Public Methods
        public void RemoveFromParent()
        {
            Container?.RemoveItem(this);
        }
        #endregion
    }
}