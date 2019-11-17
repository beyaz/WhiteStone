using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class DataEvent
    {
        #region Static Fields
        public static readonly IEvent AfterFetchedAllTableNamesInSchema = new Event {Name = nameof(AfterFetchedAllTableNamesInSchema)};
        
        
        #endregion
    }
}