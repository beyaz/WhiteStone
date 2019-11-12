using ___Company___.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;

namespace ___Company___.EntityGeneration.DataFlow
{
    /// <summary>
    ///     The data context
    /// </summary>
    public class DataContext : ___Company___.DataFlow.DataContext
    {
        #region Static Fields
        /// <summary>
        ///     The instance
        /// </summary>
        public static readonly DataContext Context = new DataContext();
        #endregion

        #region Fields
        /// <summary>
        ///     The before business class export
        /// </summary>
        public readonly IEvent BeforeBusinessClassExport = new Event {Name = nameof(BeforeBusinessClassExport)};
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        public DataContext()
        {
            AttachEvent(BeforeBusinessClassExport, SharedDalClassWriter.Write);
        }
        #endregion
    }
}