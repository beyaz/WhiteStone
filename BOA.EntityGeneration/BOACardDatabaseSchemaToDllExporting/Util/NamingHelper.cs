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
            var namingPattern = context.Get(NamingPattern.Id);
            return namingPattern.EntityNamespace;
        }

        public static string GetRepositoryNamespace(this IDataContext context)
        {
            var namingPattern = context.Get(NamingPattern.Id);
            return namingPattern.RepositoryNamespace;
            
        }
        #endregion
    }
}