
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.Models.Interfaces;

namespace BOA.EntityGeneration.DataFlow
{
    /// <summary>
    ///     The data
    /// </summary>
    static class Data
    {
        #region Static Fields

        

        #endregion
        

        #region Process Indicators
        internal static readonly Property<ProcessContract> ProcessInfo    = Property.Create<ProcessContract>(nameof(ProcessInfo));
        #endregion

        #region Files
        #endregion
    }
}