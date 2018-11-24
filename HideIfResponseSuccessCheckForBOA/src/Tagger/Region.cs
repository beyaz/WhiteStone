namespace JavaScriptRegions
{
    public class PartialRegion
    {
        #region Public Properties
        public int           Level         { get; set; }
        public PartialRegion PartialParent { get; set; }
        public int           StartLine     { get; set; }
        public int           StartOffset   { get; set; }
        #endregion
    }

    public class Region : PartialRegion
    {
        #region Public Properties
        public int EndLine { get; set; }

        public string Text { get; set; }
        #endregion
    }

}