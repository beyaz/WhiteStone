using ___Company___.EntityGeneration.DataFlow;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    /// <summary>
    ///     The naming helper
    /// </summary>
    static class NamingHelper
    {
        #region Public Methods
        /// <summary>
        ///     Gets the business class namespace.
        /// </summary>
        public static string GetBusinessClassNamespace(string schemaName)
        {
            var config = Context.Get(Data.Config);

            return config.DaoAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        /// <summary>
        ///     Gets the shared repository class namespace.
        /// </summary>
        public static string GetSharedRepositoryClassNamespace(string schemaName)
        {
            var config = Context.Get(Data.Config);

            return config.SharedClassConfig.SharedClassNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        /// <summary>
        ///     Gets the type class namespace.
        /// </summary>
        public static string GetTypeClassNamespace(string schemaName)
        {
            var config = Context.Get(Data.Config);

            return config.EntityAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }
        #endregion
    }
}