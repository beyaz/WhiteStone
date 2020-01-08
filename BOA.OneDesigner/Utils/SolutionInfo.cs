using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;

namespace BOAPlugins.TypescriptModelGeneration
{
    /// <summary>
    ///     The solution information
    /// </summary>
    [Serializable]
    public class SolutionInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the namespace name for orchestration.
        /// </summary>
        public string NamespaceNameForOrchestration { get; set; }

        /// <summary>
        ///     Gets or sets the type of the namespace name for.
        /// </summary>
        public string NamespaceNameForType { get; set; }

        /// <summary>
        ///     Gets or sets the one project folder.
        /// </summary>
        public string OneProjectFolder { get; set; }

        /// <summary>
        ///     Gets or sets the orchestration project folder.
        /// </summary>
        public string OrchestrationProjectFolder { get; set; }

        /// <summary>
        ///     Gets or sets the SLN file path.
        /// </summary>
        public string SlnFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the name of the type assembly.
        /// </summary>
        public string TypeAssemblyName { get; set; }

        /// <summary>
        ///     Gets or sets the type assembly path in server bin.
        /// </summary>
        public string TypeAssemblyPathInServerBin { get; set; }

        /// <summary>
        ///     Gets or sets the types project folder.
        /// </summary>
        public string TypesProjectFolder { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Creates from.
        /// </summary>
        public static SolutionInfo CreateFrom(string slnFilePath)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            var isSelfService = slnFilePath.Equals(@"d:\work\BOA\Dev\BOA.SelfService\BOA.SelfService.sln", StringComparison.OrdinalIgnoreCase);
            if (isSelfService)
            {
                isSelfService = true;
                slnFilePath = @"D:\work\BOA\Dev\BOA.SelfService\BOA.SelfService.Manager.sln";
            }

            var info =  new SolutionInfo
            {
                SlnFilePath                   = slnFilePath,
                TypesProjectFolder            = Path.GetDirectoryName(slnFilePath) + Path.DirectorySeparatorChar + GetNamespaceNameForType(slnFilePath) + Path.DirectorySeparatorChar,
                OrchestrationProjectFolder    = Path.GetDirectoryName(slnFilePath) + Path.DirectorySeparatorChar + GetNamespaceNameForOrchestration(slnFilePath) + Path.DirectorySeparatorChar,
                OneProjectFolder              = GetOneProjectFolder(slnFilePath),
                NamespaceNameForOrchestration = GetNamespaceNameForOrchestration(slnFilePath),
                NamespaceNameForType          = GetNamespaceNameForType(slnFilePath),
                TypeAssemblyName              = GetTypeAssemblyName(slnFilePath),
                TypeAssemblyPathInServerBin   = $@"d:\boa\server\bin\{GetTypeAssemblyName(slnFilePath)}"
            };

            if (isSelfService)
            {
                info.OneProjectFolder = @"D:\work\BOA\Dev\BOA.SelfService\One\BOA.One.Office.SelfService";
                info.OrchestrationProjectFolder = @"D:\work\BOA\Dev\BOA.SelfService\Manager\BOA.Orchestration.SelfService.Manager\";
                info.TypesProjectFolder = @"D:\work\BOA\Dev\BOA.SelfService\Manager\BOA.Types.SelfService.Manager\";
                info.SlnFilePath = @"d:\work\BOA\Dev\BOA.SelfService\BOA.SelfService.sln";
            }
            

            return info;
        }

        /// <summary>
        ///     Creates from TFS folder path.
        /// </summary>
        public static SolutionInfo CreateFromTfsFolderPath(string tfsFolderPath)
        {
            return CreateFrom(GetSlnFilePath(tfsFolderPath));
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Gets the namespace name for orchestration.
        /// </summary>
        static string GetNamespaceNameForOrchestration(string slnFilePath)
        {
            return $"BOA.Orchestration.{GetSolutionNamespaceName(slnFilePath)}";
        }

        /// <summary>
        ///     Gets the type of the namespace name for.
        /// </summary>
        static string GetNamespaceNameForType(string slnFilePath)
        {
            return $"BOA.Types.{GetSolutionNamespaceName(slnFilePath)}";
        }

        /// <summary>
        ///     Gets the one project folder.
        /// </summary>
        static string GetOneProjectFolder(string solutionFilePath)
        {
            var namespaceName = GetSolutionNamespaceName(solutionFilePath);

            var paths = new List<string>
            {
                Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One.Office." + namespaceName + Path.DirectorySeparatorChar,
                Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One." + namespaceName + Path.DirectorySeparatorChar
            };

            var packages = namespaceName.SplitAndClear(".");
            // Card.Clearing.Visa
            if (packages.Count == 3)
            {
                var namespaceName2 = packages[0] + "." + packages[1] + packages[2];

                paths.AddRange(new[]
                {
                    Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One.Office." + namespaceName2 + Path.DirectorySeparatorChar,
                    Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One." + namespaceName2 + Path.DirectorySeparatorChar
                });
            }

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    return path;
                }
            }

            return paths[0];
        }

        /// <summary>
        ///     Gets the SLN file path.
        /// </summary>
        static string GetSlnFilePath(string tfsFolderPath)
        {
            // $/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard

            var projectName = tfsFolderPath.Split('/').Last();

            return "d:\\work" + tfsFolderPath.Replace('/', '\\').RemoveFromStart("$") + Path.DirectorySeparatorChar + projectName + ".sln";
        }

        /// <summary>
        ///     Gets the name of the solution namespace.
        /// </summary>
        static string GetSolutionNamespaceName(string slnFilePath)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            return Path.GetFileName(slnFilePath).RemoveFromStart("BOA.").RemoveFromEnd(".sln");
        }

        /// <summary>
        ///     Gets the name of the type assembly.
        /// </summary>
        static string GetTypeAssemblyName(string slnFilePath)
        {
            return $"{GetNamespaceNameForType(slnFilePath)}.dll";
        }
        #endregion
    }
}