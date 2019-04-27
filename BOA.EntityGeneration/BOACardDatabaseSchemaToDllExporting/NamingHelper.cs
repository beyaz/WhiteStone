namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class NamingHelper
    {

        public string GetTypeClassNamespace(string schemaName)
        {
            return $"BOA.Types.Kernel.Card.{schemaName}";
        }

        public string GetBusinessClassNamespace(string schemaName)
        {
            return $"BOA.Business.Kernel.Card.{schemaName}";
        }
    }
}