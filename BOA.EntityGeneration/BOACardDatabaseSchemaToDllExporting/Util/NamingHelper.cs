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
        public static string GetBusinessClassNamespace(string schemaName,ConfigContract config)
        {

            return config.DaoAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        /// <summary>
        ///     Gets the shared repository class namespace.
        /// </summary>
        public static string GetSharedRepositoryClassNamespace(string schemaName, ConfigContract config)
        {

            return config.SharedRepositoryNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        /// <summary>
        ///     Gets the type class namespace.
        /// </summary>
        public static string GetTypeClassNamespace(string schemaName, ConfigContract config)
        {

            return config.EntityAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }
        #endregion
    }
}