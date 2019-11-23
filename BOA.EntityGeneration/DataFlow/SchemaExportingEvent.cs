using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class SchemaExportingEvent
    {
        #region Static Fields
        public static readonly Event SchemaExportFinished = new Event {Name = nameof(SchemaExportFinished)};
        public static readonly Event SchemaExportStarted  = new Event {Name = nameof(SchemaExportStarted)};
        #endregion
    }
}