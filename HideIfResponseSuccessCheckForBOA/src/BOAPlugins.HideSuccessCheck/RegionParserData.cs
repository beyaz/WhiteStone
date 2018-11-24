using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace BOAPlugins.HideSuccessCheck
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
}