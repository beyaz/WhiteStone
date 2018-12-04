using System;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public abstract class BField
    {
        #region Public Properties
        public string BindingPath { get;  set; }
        public string Label { get; set; }
        public string SnapName { get; set; }
        #endregion
    }

   

    [Serializable]
    public class BInputMask : BField
    {
        #region Public Properties
        public bool   IsCreditCard { get; set; }
        #endregion
    }
}