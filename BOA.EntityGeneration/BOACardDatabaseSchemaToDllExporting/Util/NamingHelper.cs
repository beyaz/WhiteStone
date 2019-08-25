using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    public class NamingHelper
    {
        #region Public Properties
        [Inject]
        public Config Config { get; set; }
        #endregion

        #region Public Methods
        public string GetBusinessClassNamespace(string schemaName)
        {
            return Config.DaoAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }

        public string GetTypeClassNamespace(string schemaName)
        {
            return Config.EntityAssemblyNamespaceFormat.Replace("{SchemaName}", schemaName);
        }
        #endregion
    }
}