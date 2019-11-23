using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class TableExportingEvent
    {
        #region Static Fields
        public static readonly Event TableExportFinished = new Event {Name = nameof(TableExportFinished)};
        public static readonly Event TableExportStarted  = new Event {Name = nameof(TableExportStarted)};
        #endregion
    }
}