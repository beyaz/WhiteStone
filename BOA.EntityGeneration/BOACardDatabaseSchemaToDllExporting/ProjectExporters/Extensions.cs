using BOA.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public static class Extensions
    {
        public static string GetEntityProjectDirectory(this IDataContext context)
        {
            var config = context.Get(Config);

            return config.EntityProjectDirectory.Replace("{SchemaName}", context.Get(SchemaName));
        }

        public static string GetRepositoryProjectDirectory(this IDataContext context)
        {
            var config = context.Get(Config);

            return config.RepositoryProjectDirectory.Replace("{SchemaName}", context.Get(SchemaName));
        }
    }
}