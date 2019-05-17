using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class MSBuildData
    {
        public string ProjectFilePath { get; set; }

        internal string BuildOutput { get; set; }
    }

    public class MSBuild
    {
        static string ProgramFilesX86()
        {
            if( 8 == IntPtr.Size 
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        public static void Build(MSBuildData data)
        {
            var msbuildPath = $@"{ProgramFilesX86()}\MSBuild\14.0\Bin\MSBuild.exe";

            ProcessStartInfo startInfo = new ProcessStartInfo(msbuildPath)
            {
                Arguments       = data.ProjectFilePath,
                ErrorDialog     = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = System.Diagnostics.Process.Start(startInfo);

            if (process == null)
            {
                throw new InvalidOperationException(nameof(process));
            }
            
            var hasError = process.ExitCode>0;
            if (hasError)
            {
                throw new InvalidOperationException(process.StandardError.ReadToEnd());
            }

            data.BuildOutput = process.StandardOutput.ReadToEnd();
        }
    }
}
