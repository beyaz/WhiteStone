using System;
using System.IO;
using System.Text;
using BOA.Common.Helpers;

namespace BOA.Tasks
{
    public static class SlnFileGenerator
    {
        #region Public Methods
        public static string GetBatFileContent(string directoryPath, string slnFileName)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (slnFileName == null)
            {
                throw new ArgumentNullException(nameof(slnFileName));
            }

            var sb = new StringBuilder();
            sb.AppendLine($"dotnet new sln --name {slnFileName}");

            foreach (var csprojFile in Directory.GetFiles(directoryPath, "-.csproj", SearchOption.AllDirectories))
            {
                var relativePath = csprojFile.RemoveFromStart(directoryPath);
                sb.AppendLine($"dotnet sln \"{slnFileName}.sln\" add \"{relativePath}\"");
            }

            sb.AppendLine("pause");

            return sb.ToString();
        }
        #endregion
    }
}