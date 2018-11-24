using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;

namespace JavaScriptRegions
{
    public class RegionParserData
    {
        #region Fields
        public readonly List<Region> Regions = new List<Region>();
        #endregion

        #region Public Properties
        public virtual int LineCount
        {
            get { return TextSnapshotLines.Count; }
        }

        public IReadOnlyList<ITextSnapshotLine> TextSnapshotLines { get; set; }
        #endregion

        #region Public Indexers
        public virtual string this[int lineIndex]
        {
            get { return TextSnapshotLines[lineIndex].GetText(); }
        }
        #endregion

        #region Public Methods
        public virtual int GetLineNumber(int lineIndex)
        {
            return TextSnapshotLines[lineIndex].LineNumber;
        }
        #endregion
    }

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