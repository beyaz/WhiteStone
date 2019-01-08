using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class SizeInfo
    {
        #region Public Properties
        public bool IsEmpty      => IsLarge == false && IsMedium == false && IsSmall == false && IsExtraSmall == false;
        public bool IsExtraSmall { get; set; }
        public bool IsLarge      { get; set; }
        public bool IsMedium     { get; set; }
        public bool IsSmall      { get; set; }
        #endregion
    }

    [Serializable]
    public class LayoutProps
    {
        #region Public Properties
        public int Wide { get; set; }
        public int X    { get; set; }
        #endregion
    }
}