using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class MSBuildData
    {
        #region Public Properties
        public string BuildOutput     { get; internal set; }
        public string ProjectFilePath { get; set; }
        public string StandardError   { get; internal set; }
        #endregion
    }

    public class MSBuild
    {
        #region Public Methods
        public static void Build(MSBuildData data)
        {
            var msbuildPath = GetMsBuildExePath();

            var arguments = new StringBuilder();

            arguments.Append(data.ProjectFilePath);

            if (data.ProjectFilePath.EndsWith(".sln"))
            {
                arguments.Append(" /p:Configuration=Debug /p:Platform=\"Any CPU\"");
            }

            var startInfo = new ProcessStartInfo(msbuildPath)
            {
                Arguments              = arguments.ToString(),
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

            process.WaitForExit();

            data.BuildOutput   = process.StandardOutput.ReadToEnd();
            data.StandardError = process.StandardError.ReadToEnd();

            var hasError = process.ExitCode > 0;
            if (hasError)
            {
                throw new InvalidOperationException(JsonHelper.Serialize(data));
            }
        }

        public static string GetBatFileContent(MSBuildData data)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"SET PATH={Path.GetDirectoryName(GetMsBuildExePath()) + Path.DirectorySeparatorChar}");

            sb.AppendLine($@"SET SolutionPath={data.ProjectFilePath}");

            if (data.ProjectFilePath.EndsWith(".sln"))
            {
                sb.AppendLine(@"MSbuild %SolutionPath% /p:Configuration=Debug /p:Platform=""Any CPU""");
            }
            else
            {
                sb.AppendLine(@"MSbuild %SolutionPath%");
            }

            return sb.ToString();
        }
        #endregion

        #region Methods
        static string GetMsBuildExePath()
        {
            var msbuildPath = $@"{ProgramFilesX86()}\MSBuild\14.0\Bin\MSBuild.exe";
            if (File.Exists(msbuildPath))
            {
                return msbuildPath;
            }

            msbuildPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
            if (File.Exists(msbuildPath))
            {
                return msbuildPath;
            }

            throw new InvalidOperationException($"{msbuildPath} is not found.");
        }

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