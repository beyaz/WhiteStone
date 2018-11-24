using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace JavaScriptRegions
{
    internal class RegionClassifier : IClassifier
    {
        #region Static Fields
        static readonly Regex _regex = new Regex(@"^\s*//\s*(?<region>#(end)?region.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Fields
        readonly IClassificationType _formatDefinition;
        #endregion

        #region Constructors
        public RegionClassifier(IClassificationTypeRegistryService registry)
        {
            _formatDefinition = registry.GetClassificationType(PredefinedClassificationTypeNames.ExcludedCode);
        }
        #endregion

        #region Public Events
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged
        {
            add { }
            remove { }
        }
        #endregion

        #region Public Methods
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            return new List<ClassificationSpan>();
            
            //var list = new List<ClassificationSpan>();
            //var text = span.GetText();

            //if (span.IsEmpty || string.IsNullOrWhiteSpace(text))
            //{
            //    return list;
            //}

            //var match = _regex.Match(text);

            //if (match.Success)
            //{
            //    var group  = match.Groups["region"];
            //    var result = new SnapshotSpan(span.Snapshot, span.Start + group.Index, group.Length);
            //    list.Add(new ClassificationSpan(result, _formatDefinition));
            //}

            //return list;
        }
        #endregion
    }
}