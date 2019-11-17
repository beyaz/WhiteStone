using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class TableExportingEvent
    {
        public static readonly IEvent TableExportStarted  = new Event {Name = nameof(TableExportStarted)};
        public static readonly IEvent TableExportFinished = new Event {Name = nameof(TableExportFinished)};
    }
}