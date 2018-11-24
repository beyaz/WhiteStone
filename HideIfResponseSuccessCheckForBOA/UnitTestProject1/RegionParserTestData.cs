using System;
using System.Collections.Generic;
using System.Linq;
using BOAPlugins.HideSuccessCheck;

namespace JavaScriptRegions
{
    public class RegionParserTestData : RegionParserData
    {
        #region Public Properties
        public override int LineCount
        {
            get { return SourceTextLines.Count; }
        }

        public string SourceText
        {
            set { SourceTextLines = value.Split(Environment.NewLine.ToCharArray()).Where(x => string.IsNullOrWhiteSpace(x) == false).ToList(); }
        }

        public IReadOnlyList<string> SourceTextLines { get; set; }
        #endregion

        #region Public Indexers
        public override string this[int lineIndex]
        {
            get { return SourceTextLines[lineIndex]; }
        }
        #endregion

        #region Public Methods
        public override int GetLineNumber(int lineIndex)
        {
            return 0;
        }
        #endregion
    }
}