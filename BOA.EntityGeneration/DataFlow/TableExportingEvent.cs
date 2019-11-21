using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class TableExportingEvent
    {
        #region Static Fields
        public static readonly IEvent TableExportFinished = new Event {Name = nameof(TableExportFinished)};
        public static readonly IEvent TableExportStarted  = new Event {Name = nameof(TableExportStarted)};
        #endregion
    }
}