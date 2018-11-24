namespace JavaScriptRegions
{
    public class Region
    {
        #region Public Properties
        public int    EndLine       { get; set; }
        public int    Level         { get; set; }
        public Region PartialParent { get; set; }
        public int    StartLine     { get; set; }
        public int    StartOffset   { get; set; }
        public string Text          { get; set; }
        #endregion
    }
}