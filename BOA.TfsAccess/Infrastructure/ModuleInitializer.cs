namespace BOA.Infrastructure
{
    public static class ModuleInitializer
    {
        #region Public Methods
        public static void Initialize()
        {
            var references = new[]
            {
                "Microsoft.TeamFoundation.Core.WebApi.dll",
                "Microsoft.TeamFoundation.Work.WebApi.dll",
                "Microsoft.TeamFoundation.WorkItemTracking.WebApi.dll",
                "Microsoft.TeamFoundation.Diff.dll",
                "Microsoft.TeamFoundation.VersionControl.Client.dll",
                "Microsoft.TeamFoundation.VersionControl.Common.dll",
                "Microsoft.TeamFoundation.VersionControl.Common.Integration.dll",
                "Microsoft.TeamFoundation.WorkItemTracking.Client.DataStoreLoader.dll",
                "Microsoft.TeamFoundation.WorkItemTracking.Client.dll",
                "Microsoft.TeamFoundation.WorkItemTracking.Client.QueryLanguage.dll",
                "Microsoft.TeamFoundation.WorkItemTracking.Common.dll",
                "Microsoft.TeamFoundation.WorkItemTracking.Proxy.dll",
                "Microsoft.TeamFoundation.Client.dll",
                "Microsoft.TeamFoundation.Common.dll",
                "Microsoft.VisualStudio.Services.WebApi.dll",
                "Microsoft.VisualStudio.Services.Common.dll",
                "Microsoft.VisualStudio.Services.Client.dll",
                "Microsoft.ServiceBus.dll",
                "System.Net.Http.Formatting.dll",
                "System.IdentityModel.Tokens.Jwt.dll"
            };

            EmbeddedAssemblyReferenceResolver.Resolve(references);
        }
        #endregion
    }
}