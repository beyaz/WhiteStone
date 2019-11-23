using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class TableExportingEvent
    {
        #region Static Fields
        public static readonly Event TableExportFinished = Event.Create( nameof(TableExportFinished));
        public static readonly Event TableExportStarted  = Event.Create( nameof(TableExportStarted));
        #endregion
    }
}