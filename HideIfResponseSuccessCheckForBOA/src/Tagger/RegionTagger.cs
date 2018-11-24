using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace JavaScriptRegions
{
    internal sealed class RegionTagger : ITagger<IOutliningRegionTag>
    {
        #region Fields
        readonly ITextBuffer _buffer;
        List<Region>         _regions;
        ITextSnapshot        _snapshot;
        #endregion

        #region Constructors
        public RegionTagger(ITextBuffer buffer)
        {
            _buffer         =  buffer;
            _snapshot       =  buffer.CurrentSnapshot;
            _regions        =  new List<Region>();
            _buffer.Changed += BufferChanged;

            ThreadHelper.Generic.BeginInvoke(DispatcherPriority.ApplicationIdle, ReParse);

            _buffer.Changed += BufferChanged;
        }
        #endregion

        #region Public Events
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
        #endregion

        #region Public Methods
        public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            try
            {
                if (spans.Count == 0)
                {
                    return Enumerable.Empty<ITagSpan<IOutliningRegionTag>>();
                }

                var currentRegions  = _regions;
                var currentSnapshot = _snapshot;

                var entire          = new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End).TranslateTo(currentSnapshot, SpanTrackingMode.EdgeExclusive);
                var startLineNumber = entire.Start.GetContainingLine().LineNumber;
                var endLineNumber   = entire.End.GetContainingLine().LineNumber;
                var tags            = new List<ITagSpan<IOutliningRegionTag>>();
                foreach (var region in currentRegions)
                {
                    if (region.StartLine <= endLineNumber && region.EndLine >= startLineNumber)
                    {
                        var startLine = currentSnapshot.GetLineFromLineNumber(region.StartLine);
                        var endLine   = currentSnapshot.GetLineFromLineNumber(region.EndLine);

                        var snapshot = new SnapshotSpan(startLine.Start + region.StartOffset, endLine.End);

                        //the region starts at the beginning of the "[", and goes until the *end* of the line that contains the "]".
                        tags.Add(new TagSpan<IOutliningRegionTag>(snapshot,
                                                                  new OutliningRegionTag(true, true, region.Text, snapshot.GetText())));
                    }
                }

                return tags;
            }
            catch (Exception e)
            {
                Log.Push(e);
                return Enumerable.Empty<ITagSpan<IOutliningRegionTag>>();
            }
        }
        #endregion

        #region Methods
        static SnapshotSpan AsSnapshotSpan(Region region, ITextSnapshot snapshot)
        {
            var startLine = snapshot.GetLineFromLineNumber(region.StartLine);
            var endLine = region.StartLine == region.EndLine
                ? startLine
                : snapshot.GetLineFromLineNumber(region.EndLine);
            var snapshotSpan = new SnapshotSpan(startLine.Start + region.StartOffset, endLine.End);

            return snapshotSpan;
        }

        static bool TryGetLevel(string text, int startIndex, out int level)
        {
            level = -1;
            if (text.Length > startIndex + 3)
            {
                if (int.TryParse(text.Substring(startIndex + 1), out level))
                {
                    return true;
                }
            }

            return false;
        }

        void BufferChanged(object sender, TextContentChangedEventArgs e)
        {
            // If this isn't the most up-to-date version of the buffer, then ignore it for now (we'll eventually get another change event).
            if (e.After != _buffer.CurrentSnapshot)
            {
                return;
            }

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(ReParse), DispatcherPriority.ApplicationIdle, null);
        }

        void ReParse()
        {
            try
            {
                var newSnapshot = _buffer.CurrentSnapshot;

                var textSnapshotLines = newSnapshot.Lines.ToList();

                var data = new RegionParserData
                {
                    TextSnapshotLines = textSnapshotLines
                };

                RegionParser.Parse(data);

                var newRegions = data.Regions;



                //determine the changed span, and send a changed event with the new spans
                var oldSpans = new List<Span>(_regions.Select(r => AsSnapshotSpan(r, _snapshot)
                                                                   .TranslateTo(newSnapshot, SpanTrackingMode.EdgeExclusive)
                                                                   .Span));
                var newSpans = new List<Span>(newRegions.Select(r => AsSnapshotSpan(r, newSnapshot).Span));

                var oldSpanCollection = new NormalizedSpanCollection(oldSpans);
                var newSpanCollection = new NormalizedSpanCollection(newSpans);

                //the changed regions are regions that appear in one set or the other, but not both.
                var removed = NormalizedSpanCollection.Difference(oldSpanCollection, newSpanCollection);

                var changeStart = int.MaxValue;
                var changeEnd   = -1;

                if (removed.Count > 0)
                {
                    changeStart = removed[0].Start;
                    changeEnd   = removed[removed.Count - 1].End;
                }

                if (newSpans.Count > 0)
                {
                    changeStart = Math.Min(changeStart, newSpans[0].Start);
                    changeEnd   = Math.Max(changeEnd, newSpans[newSpans.Count - 1].End);
                }

                _snapshot = newSnapshot;
                _regions  = newRegions;

                if (changeStart <= changeEnd)
                {
                    TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(_snapshot, Span.FromBounds(changeStart, changeEnd))));
                }
            }
            catch (Exception e)
            {
                Log.Push(e);
            }
        }
        #endregion
    }
}