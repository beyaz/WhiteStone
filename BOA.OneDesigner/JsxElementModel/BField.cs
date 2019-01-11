using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGeneration;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public abstract class BField
    {

        public string SnapName { get; set; }

        public Container Container { get; set; }

        public void RemoveFromParent()
        {
            Container?.RemoveItem(this);
        }

        public string Indent { get; set; }

        #region Public Properties

        public string BindingPath { get; set; }

        public string BindingPathInTypeScript=> TypescriptNaming.NormalizeBindingPath(RenderHelper.BindingPrefix + BindingPath);



        public string Label       { get; set; }

        public string SnapName2
        {
            get
            {
                var typeName = GetType().Name.RemoveFromStart("B");

                return typeName[0].ToString().ToLowerTR() + typeName.Substring(1) + BindingPath;
            }
        }
        #endregion
    }
}