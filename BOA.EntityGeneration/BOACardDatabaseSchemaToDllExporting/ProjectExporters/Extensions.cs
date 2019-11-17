using BOA.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public static class Extensions
    {
        public static string GetEntityProjectDirectory(this IDataContext context)
        {
            var namingPattern = context.Get(NamingPattern.Id);

            return namingPattern.EntityProjectDirectory;
        }

        public static string GetRepositoryProjectDirectory(this IDataContext context)
        {
            var namingPattern = context.Get(NamingPattern.Id);

            return namingPattern.RepositoryProjectDirectory;
        }
    }
}