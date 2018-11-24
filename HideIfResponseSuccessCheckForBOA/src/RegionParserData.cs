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
        public int LineCount
        {
            get
            {
                if (SourceTextLines != null)
                {
                    return SourceTextLines.Count;
                }

                return TextSnapshotLines.Count;
            }
        }

        public string SourceText
        {
            set { SourceTextLines = value.Split(Environment.NewLine.ToCharArray()).Where(x => string.IsNullOrWhiteSpace(x) == false).ToList(); }
        }

        public IReadOnlyList<string> SourceTextLines { get; set; }

        public IReadOnlyList<ITextSnapshotLine> TextSnapshotLines { get; set; }
        #endregion

        #region Public Indexers
        public string this[int lineIndex]
        {
            get
            {
                if (SourceTextLines != null)
                {
                    return SourceTextLines[lineIndex];
                }

                return TextSnapshotLines[lineIndex].GetText();
            }
        }
        #endregion

        #region Public Methods
        public int GetLineNumber(int lineIndex)
        {
            if (SourceTextLines != null)
            {
                return 0;
            }

            return TextSnapshotLines[lineIndex].LineNumber;
        }
        #endregion
    }
}