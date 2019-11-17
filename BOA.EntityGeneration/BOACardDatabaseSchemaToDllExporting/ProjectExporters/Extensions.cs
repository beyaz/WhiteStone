using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public static class Extensions
    {
        public static string GetEntityProjectDirectory(this IDataContext context)
        {
            var namingPattern = context.Get(Data.NamingPattern);

            return namingPattern.EntityProjectDirectory;
        }

        public static string GetRepositoryProjectDirectory(this IDataContext context)
        {
            var namingPattern = context.Get(Data.NamingPattern);

            return namingPattern.RepositoryProjectDirectory;
        }
    }
}