using System;
using BOA.Common.Helpers;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public abstract class BField
    {
        #region Public Properties
        public string BindingPath { get; set; }
        public string Label       { get; set; }

        public string SnapName
        {
            get
            {
                var typeName = GetType().Name.RemoveFromStart("B");

                return typeName[0].ToString().ToLowerTR() + typeName.Substring(1) + BindingPath;
            }
        }
        #endregion
    }

    [Serializable]
    public class BInputMask : BField
    {
        #region Public Properties
        public bool IsCreditCard { get; set; }
        #endregion
    }
}