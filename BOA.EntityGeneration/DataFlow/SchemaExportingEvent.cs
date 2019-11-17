using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class SchemaExportingEvent
    {
        public static readonly IEvent SchemaExportStarted  = new Event {Name = nameof(SchemaExportStarted)};
        public static readonly IEvent SchemaExportFinished = new Event {Name = nameof(SchemaExportFinished)};
    }
}