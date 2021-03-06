﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Infrastructure;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BOA.TfsAccess
{
    public class TFSAccessForBOA
    {
        #region Constructors
        static TFSAccessForBOA()
        {
            ModuleInitializer.Initialize();
        }
        #endregion

        #region Properties
        internal static TfsTeamProjectCollection KT => TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://srvtfs:8080/tfs/KT"));
        #endregion

        #region Public Methods
        public static void CheckInFile(string path, string comment)
        {
            var tfsServerUri = GetTfsServerPath(path);

            var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsServerUri));

            var workspace = GetWorkspaceOfSolution(pc, path);

            var change = workspace?.GetPendingChangesEnumerable().Where(p => p.LocalItem.StartsWith(path, StringComparison.OrdinalIgnoreCase)).ToArray();
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

            var workspace = GetWorkspaceOfSolution(pc, solutionFilePath);

            if (workspace == null)
            {
                input.ResultMessage = "Workspace not found. @SolutionFilePath: " + input.SolutionFilePath;
                return;
            }

            var change = workspace.GetPendingChangesEnumerable().Where(p => p.LocalItem.StartsWith(solutionFileDirectory, StringComparison.OrdinalIgnoreCase)).ToArray();
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

        public static string CheckoutFile(string path)
        {
            var uri = GetTfsServerPath(path);

            using (var teamProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(uri)))
            {
                if (teamProjectCollection == null)
                {
                    return Messages.IsNull(nameof(teamProjectCollection));
                }

                var workspace = GetWorkspaceOfSolution(teamProjectCollection, path);

                if (workspace == null)
                {
                    return Messages.IsNull(nameof(workspace));
                }

                var count = workspace.PendEdit(path, RecursionType.Full);
                if (count == 1)
                {
                    return null;
                }

                return "Number of checked files is " + count;
            }
        }

        public static void DownloadFile(string path, string destinationPath)
        {
            var ConstTfsServerUri = GetTfsServerPath(path);

            using (var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(ConstTfsServerUri)))
            {
                if (pc == null)
                {
                    throw new Exception(nameof(pc));
                }

                var version = pc.GetService(typeof(VersionControlServer)) as VersionControlServer;
                var item    = version?.GetItem(path);
                var stream  = item?.DownloadFile();
                if (stream == null)
                {
                    throw new InvalidOperationException(path);
                }

                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                var directoryName = Path.GetDirectoryName(destinationPath);
                if (directoryName == null)
                {
                    throw new ArgumentNullException(nameof(directoryName));
                }

                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                using (var fs = new FileStream(destinationPath, FileMode.OpenOrCreate))
                {
                    stream.ReadAllWriteToOutput(fs);
                }
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

        public static IReadOnlyList<string> GetSubFolderNames(string tfsPathWithSearchPattern)
        {
            var ConstTfsServerUri = GetTfsServerPath(tfsPathWithSearchPattern);

            using (var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(ConstTfsServerUri)))
            {
                if (pc == null)
                {
                    return null;
                }

                var version = pc.GetService(typeof(VersionControlServer)) as VersionControlServer;

                var items = version?.GetItems(tfsPathWithSearchPattern);

                return items?.Items.Select(x => x.ServerItem).ToList();
            }
        }

        public static bool HasFile(string path)
        {
            var ConstTfsServerUri = GetTfsServerPath(path);

            using (var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(ConstTfsServerUri)))
            {
                if (pc == null)
                {
                    return false;
                }

                var version = pc.GetService(typeof(VersionControlServer)) as VersionControlServer;

                return version?.ServerItemExists(path, ItemType.Any) == true;
            }
        }

        public static bool UndoCheckoutFile(string path)
        {
            var ConstTfsServerUri = GetTfsServerPath(path);

            using (var pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(ConstTfsServerUri)))
            {
                if (pc == null)
                {
                    return false;
                }

                var workspace = GetWorkspaceOfSolution(pc, path);

                return workspace?.Undo(path, RecursionType.Full) == 1;
            }
        }
        #endregion

        #region Methods
        internal static void CreateWorkspace(TfsTeamProjectCollection tfsTeamProjectCollection, string name, string serverPath, string localPath)
        {
            var _versionControl = tfsTeamProjectCollection.GetService<VersionControlServer>();

            var _workspace = _versionControl.CreateWorkspace(name, _versionControl.AuthorizedUser);

            _workspace.Map(serverPath, localPath);
        }

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

        static Workspace GetWorkspaceOfSolution(TfsTeamProjectCollection pc, string filePathInMappedFolder)
        {
            var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(filePathInMappedFolder);

            return workspaceInfo?.GetWorkspace(pc);
        }
        #endregion

        static class Messages
        {
            #region Public Methods
            public static string IsNull(string parameter)
            {
                return parameter + " is null";
            }
            #endregion
        }
    }
}