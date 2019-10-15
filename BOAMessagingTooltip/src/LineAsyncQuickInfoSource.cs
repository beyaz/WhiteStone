using System.Threading;
using System.Threading.Tasks;
using BOAMessagingTooltip;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;

namespace AsyncQuickInfo
{
    internal sealed class LineAsyncQuickInfoSource : IAsyncQuickInfoSource
    {
        #region Static Fields
        static readonly ImageId _icon = KnownMonikers.AbstractCube.ToImageId();
        #endregion

        #region Fields
        readonly ITextBuffer _textBuffer;
        #endregion

        #region Constructors
        public LineAsyncQuickInfoSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            // This provider does not perform any cleanup.
        }

        // This is called on a background thread.
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            var triggerPoint = session.GetTriggerPoint(_textBuffer.CurrentSnapshot);

            if (triggerPoint == null)
            {
                return Task.FromResult<QuickInfoItem>(null);
            }

            var line = triggerPoint.Value.GetContainingLine();

            var data = new ExecutionData
            {
                Line = line.GetText()
            };
            Execution.Process(data);

            if (!data.LineParsedSuccessFully)
            {
                return Task.FromResult<QuickInfoItem>(null);
            }

            Log(data);

            var lineSpan = _textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

            var lineNumberElm = new ContainerElement(
                                                     ContainerElementStyle.Stacked,
                                                     new ClassifiedTextElement(
                                                                               new ClassifiedTextRun(PredefinedClassificationTypeNames.SymbolDefinition, "TR: "),
                                                                               new ClassifiedTextRun(PredefinedClassificationTypeNames.Comment, data.MessagingAccessInfo.TurkishText)
                                                                              )
                                                    );

            return Task.FromResult(new QuickInfoItem(lineSpan, lineNumberElm));
        }
        #endregion

        #region Methods
        static void Log(ExecutionData data)
        {
            //foreach (var trace in data.Trace)
            //{
            //    BOA.Common.Helpers.Log.Push(trace);
            //}
        }
        #endregion
    }
}