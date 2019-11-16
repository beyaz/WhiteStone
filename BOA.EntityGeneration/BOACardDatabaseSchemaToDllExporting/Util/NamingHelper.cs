using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    /// <summary>
    ///     The naming helper
    /// </summary>
    static class NamingHelper
    {
        #region Public Methods
       

        /// <summary>
        ///     Gets the shared repository class namespace.
        /// </summary>
        public static string GetSharedRepositoryClassNamespace(string schemaName, ConfigContract config)
        {
            return config.SharedRepositoryNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        

        public static string GetEntityNamespace(this IDataContext context)
        {
            var config = context.Get(Data.Config);

            return config.EntityAssemblyNamespaceFormat.Replace("{SchemaName}", context.Get(Data.SchemaName));
        }

        public static string GetRepositoryNamespace(this IDataContext context)
        {
            var config = context.Get(Data.Config);

            return config.DaoAssemblyNamespaceFormat.Replace("{SchemaName}", context.Get(Data.SchemaName));
        }
        #endregion
    }
}