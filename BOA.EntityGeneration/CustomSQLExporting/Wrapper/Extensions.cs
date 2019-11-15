using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    static class Extensions
    {
        #region Public Methods
        public static string GetNameofEntityNamespace(this IDataContext context)
        {
            var config    = context.Get(Data.Config);
            var profileId = context.Get(CustomSqlExporter.ProfileId);

            return config.CustomSQLEntityNamespaceFormat.Replace("{ProfileId}", profileId);
        }

        public static string GetNameofRepositoryNamespace(this IDataContext context)
        {
            var config    = context.Get(Data.Config);
            var profileId = context.Get(CustomSqlExporter.ProfileId);

            return config.CustomSQLRepositoryNamespaceFormat.Replace("{ProfileId}", profileId);
        }
        #endregion
    }
}