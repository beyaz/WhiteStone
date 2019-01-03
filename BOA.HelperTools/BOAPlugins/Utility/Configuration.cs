using System;
using System.Collections.Generic;

namespace BOAPlugins.Utility
{
    [Serializable]
    public class CheckInInformation
    {
        public Dictionary<string, string> SolutionCheckInComments { get; set; } = new Dictionary<string, string>();
    }


    [Serializable]
    public class Configuration
    {
        #region Public Properties
        public bool CheckInSolutionIsEnabled     { get; set; }
        public bool DeepEndsAssembliesDownloaded { get; set; }

        public IReadOnlyCollection<string> ServerFiles { get; set; } = new[]
        {
            "Newtonsoft.Json.dll",
            "ClangSharp.dll",
            "DeepEnds.Console.exe",
            "DeepEnds.Console.exe.config",
            "DeepEnds.Core.dll",
            "DeepEnds.Cpp.dll",
            "DeepEnds.CSharp.dll",
            "DeepEnds.Decompile.dll",
            "DeepEnds.DGML.dll",
            "DeepEnds.DoxygenXml.dll",
            "DeepEnds.Reporting.dll",
            "DeepEnds.VBasic.dll",
            "libclang.dll",
            "Microsoft.CodeAnalysis.CSharp.dll",
            "Microsoft.CodeAnalysis.dll",
            "Microsoft.CodeAnalysis.VisualBasic.dll",
            "Microsoft.VisualStudio.Diagnostics.PerformanceProvider.dll",
            "Microsoft.VisualStudio.GraphModel.dll",
            "Microsoft.VisualStudio.OLE.Interop.dll",
            "Mono.Cecil.dll",
            "System.Collections.Immutable.dll",
            "System.Reflection.Metadata.dll"
        };
        #endregion
    }
}