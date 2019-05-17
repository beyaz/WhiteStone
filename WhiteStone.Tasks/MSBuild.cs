using System;
using System.Diagnostics;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class MSBuildData
    {
        #region Public Properties
        public string ProjectFilePath { get; set; }
        #endregion

        #region Properties
        internal string BuildOutput { get; set; }
        #endregion
    }

    public class MSBuild
    {
        #region Public Methods
        public static void Build(MSBuildData data)
        {
            var msbuildPath = $@"{ProgramFilesX86()}\MSBuild\14.0\Bin\MSBuild.exe";

            var startInfo = new ProcessStartInfo(msbuildPath)
            {
                Arguments              = data.ProjectFilePath,
                ErrorDialog            = true,
                UseShellExecute        = false,
                RedirectStandardOutput = true,
                RedirectStandardError  = true
            };

            var process = Process.Start(startInfo);

            if (process == null)
            {
                throw new InvalidOperationException(nameof(process));
            }

            var hasError = process.ExitCode > 0;
            if (hasError)
            {
                throw new InvalidOperationException(process.StandardError.ReadToEnd());
            }

            data.BuildOutput = process.StandardOutput.ReadToEnd();
        }
        #endregion

        #region Methods
        static string ProgramFilesX86()
        {
            if (8 == IntPtr.Size
                || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
        #endregion
    }
}