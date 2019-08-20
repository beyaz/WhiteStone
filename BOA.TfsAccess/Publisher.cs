using BOA.Common.Helpers;

namespace BOA.TfsAccess
{
    public class Publisher
    {
        #region Public Methods
        public static void Zip()
        {
            const string binDebug = "D:\\github\\WhiteStone\\BOA.TfsAccess\\bin\\Debug\\";

            var files = new[]
            {
                binDebug + "WhiteStone.dll",
                binDebug + "BOA.TfsAccess.dll",
                binDebug + "Newtonsoft.Json.dll",
                binDebug + "Mono.Cecil.dll",
                binDebug + "Mono.Cecil.Mdb.dll",
                binDebug + "Mono.Cecil.Pdb.dll",
                binDebug + "Mono.Cecil.Rocks.dll",
                binDebug + "Microsoft.TeamFoundation.Core.WebApi.dll",
                binDebug + "Microsoft.TeamFoundation.Work.WebApi.dll",
                binDebug + "Microsoft.TeamFoundation.WorkItemTracking.WebApi.dll",
                binDebug + "Microsoft.TeamFoundation.Diff.dll",
                binDebug + "Microsoft.TeamFoundation.VersionControl.Client.dll",
                binDebug + "Microsoft.TeamFoundation.VersionControl.Common.dll",
                binDebug + "Microsoft.TeamFoundation.VersionControl.Common.Integration.dll",
                binDebug + "Microsoft.TeamFoundation.WorkItemTracking.Client.DataStoreLoader.dll",
                binDebug + "Microsoft.TeamFoundation.WorkItemTracking.Client.dll",
                binDebug + "Microsoft.TeamFoundation.WorkItemTracking.Client.QueryLanguage.dll",
                binDebug + "Microsoft.TeamFoundation.WorkItemTracking.Common.dll",
                binDebug + "Microsoft.TeamFoundation.WorkItemTracking.Proxy.dll",
                binDebug + "Microsoft.TeamFoundation.Client.dll",
                binDebug + "Microsoft.TeamFoundation.Common.dll",
                binDebug + "Microsoft.VisualStudio.Services.WebApi.dll",
                binDebug + "Microsoft.VisualStudio.Services.Common.dll",
                binDebug + "Microsoft.VisualStudio.Services.Client.dll",
                binDebug + "Microsoft.ServiceBus.dll",
                binDebug + "System.Net.Http.Formatting.dll",
                binDebug + "Microsoft.SqlServer.TransactSql.ScriptDom.dll",
                binDebug + "System.IdentityModel.Tokens.Jwt.dll"
            };
            ZipHelper.CompressFiles("D:\\github\\WhiteStone\\bin\\BOA.TfsAccess.zip", files);
        }
        #endregion
    }
}