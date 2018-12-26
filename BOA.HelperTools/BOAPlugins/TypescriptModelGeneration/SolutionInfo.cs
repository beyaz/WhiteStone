using System;
using System.IO;
using BOA.Common.Helpers;

namespace BOAPlugins.TypescriptModelGeneration
{
    [Serializable]
    public class SolutionInfo
    {
        #region Public Properties
        public string NamespaceNameForOrchestration { get; set; }
        public string NamespaceNameForType          { get; set; }
        public string OneProjectFolder              { get; set; }
        public string OrchestrationProjectFolder    { get; set; }
        public string SlnFilePath                   { get; set; }

        public string TypeAssemblyName   { get; set; }
        public string TypesProjectFolder { get; set; }
        #endregion

        #region Public Methods
        public static SolutionInfo CreateFrom(string slnFilePath)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            return new SolutionInfo
            {
                SlnFilePath                   = slnFilePath,
                TypesProjectFolder            = Path.GetDirectoryName(slnFilePath) + Path.DirectorySeparatorChar + GetNamespaceNameForType(slnFilePath) + Path.DirectorySeparatorChar,
                OrchestrationProjectFolder    = Path.GetDirectoryName(slnFilePath) + Path.DirectorySeparatorChar + GetNamespaceNameForOrchestration(slnFilePath) + Path.DirectorySeparatorChar,
                OneProjectFolder              = GetOneProjectFolder(slnFilePath),
                NamespaceNameForOrchestration = GetNamespaceNameForOrchestration(slnFilePath),
                NamespaceNameForType          = GetNamespaceNameForType(slnFilePath),
                TypeAssemblyName              = GetTypeAssemblyName(slnFilePath)
            };
        }
        #endregion

        #region Methods
        static string GetNamespaceNameForOrchestration(string slnFilePath)
        {
            return $"BOA.Orchestration.{GetSolutionNamespaceName(slnFilePath)}";
        }

        static string GetNamespaceNameForType(string slnFilePath)
        {
            return $"BOA.Types.{GetSolutionNamespaceName(slnFilePath)}";
        }

        static string GetOneProjectFolder(string solutionFilePath)
        {
            var namespaceName = GetSolutionNamespaceName(solutionFilePath);

            var paths = new[]
            {
                Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One.Office." + namespaceName + Path.DirectorySeparatorChar,
                Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One." + namespaceName + Path.DirectorySeparatorChar
            };

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    return path;
                }
            }

            return paths[0];
        }

        static string GetSolutionNamespaceName(string slnFilePath)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            return Path.GetFileName(slnFilePath).RemoveFromStart("BOA.").RemoveFromEnd(".sln");
        }

        static string GetTypeAssemblyName(string slnFilePath)
        {
            return $"{GetNamespaceNameForType(slnFilePath)}.dll";
        }
        #endregion
    }
}