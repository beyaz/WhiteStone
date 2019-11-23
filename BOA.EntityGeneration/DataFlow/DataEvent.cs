using BOA.DataFlow;

namespace BOA.EntityGeneration.DataFlow
{
    static class DataEvent
    {
        #region Static Fields
        public static readonly Event AfterFetchedAllTableNamesInSchema =Event.Create(nameof(AfterFetchedAllTableNamesInSchema));
        #endregion
    }
}