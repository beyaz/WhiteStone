using System;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BOA.CodeGeneration.Util
{
    public class CheckInSolutionInput
    {
        #region Public Properties
        public string Comment          { get; set; }
        public string ResultMessage    { get; set; }
        public string SolutionFilePath { get; set; }
        #endregion
    }

    public class TFSAccessForBOA
    {
        #region Public Methods
        public static void CheckInFile(string path, string comment)
        {
            var tfsServerUri = GetTfsServerPath(path);

            var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsServerUri));

            var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(path);
            var workspace     = workspaceInfo?.GetWorkspace(pc);

            var change = workspace?.GetPendingChangesEnumerable().Where(p => p.LocalItem.ToUpperEN() == path.ToUpperEN()).ToArray();
            if (change?.Any() == true)
            {
                workspace.CheckIn(change.ToArray(), comment);
            }
        }

        public static void CheckInSolution(CheckInSolutionInput input)
        {
            var solutionFilePath = input.SolutionFilePath;
            var comment          = input.Comment;

            var solutionFileDirectory = Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar;

            var tfsServerUri = GetTfsServerPath(solutionFilePath);

            var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsServerUri));

            var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(solutionFilePath);

            var workspace = workspaceInfo?.GetWorkspace(pc);

            if (workspace == null)
            {
                input.ResultMessage = "Workspace not found. @SolutionFilePath: " + input.SolutionFilePath;
                return;
            }

            var change = workspace.GetPendingChangesEnumerable().Where(p => p.LocalItem.ToUpperEN().StartsWith(solutionFileDirectory.ToUpperEN())).ToArray();
            if (change.Any())
            {
                workspace.CheckIn(change.ToArray(), comment);

                input.ResultMessage = "Successfully checked in. @File Count: " + change.Length;
            }
            else
            {
                input.ResultMessage = "There is no pending changes in solution: " + solutionFileDirectory;
            }
        }

        public static bool CheckoutFile(string path)
        {
            var ConstTfsServerUri = GetTfsServerPath(path);

            using (var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(ConstTfsServerUri)))
            {
                if (pc == null)
                {
                    return false;
                }

                var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(path);
                var workspace     = workspaceInfo?.GetWorkspace(pc);
                return workspace?.PendEdit(path, RecursionType.Full) == 1;
            }
        }

        public static string GetFileContent(string path)
        {
            var ConstTfsServerUri = GetTfsServerPath(path);

            using (var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(ConstTfsServerUri)))
            {
                if (pc == null)
                {
                    return null;
                }

                var version = pc.GetService(typeof(VersionControlServer)) as VersionControlServer;
                var item    = version?.GetItem(path);
                var stream  = item?.DownloadFile();
                if (stream == null)
                {
                    return null;
                }

                return new StreamReader(stream).ReadToEnd();
            }
        }
        #endregion

        #region Methods
        static string GetTfsServerPath(string path)
        {
            if (path.IndexOf("WORKDE", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "http://srvtfs:8080/tfs/DE";
            }

            if (path.IndexOf("WORKEMLAK", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "http://srvtfs:8080/tfs/EMLAK";
            }

            return "http://srvtfs:8080/tfs/KT";
        }
        #endregion
    }
}