using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    /// <summary>
    ///     The naming helper
    /// </summary>
    public class NamingHelper
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        [Inject]
        public Config Config { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the business class namespace.
        /// </summary>
        public string GetBusinessClassNamespace(string schemaName)
        {
            return Config.DaoAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        /// <summary>
        ///     Gets the type class namespace.
        /// </summary>
        public string GetTypeClassNamespace(string schemaName)
        {
            return Config.EntityAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        public string GetSharedRepositoryClassNamespace(string schemaName)
        {
            return Config.SharedClassConfig.SharedClassNamespaceFormat.Replace("{SchemaName}", schemaName);
        }
        #endregion
    }
}