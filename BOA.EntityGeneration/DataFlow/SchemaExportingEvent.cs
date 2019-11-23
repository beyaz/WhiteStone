using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class SchemaExportingEvent
    {
        #region Static Fields
        public static readonly Event SchemaExportFinished = Event.Create(nameof(SchemaExportFinished));
        public static readonly Event SchemaExportStarted  = Event.Create(nameof(SchemaExportStarted));
        #endregion
    }
}