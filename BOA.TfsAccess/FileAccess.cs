using System;
using System.IO;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;

namespace BOA.TfsAccess
{
    public class FileAccess
    {
        #region Public Methods
        public FileAccessWriteResult WriteAllText(string path, string content)
        {
            var tfsPath = GetTfsPath(path);

            var oldContent = string.Empty;
            if (TFSAccessForBOA.HasFile(tfsPath))
            {
                oldContent = TFSAccessForBOA.GetFileContent(tfsPath);
            }

            var isEqual = StringHelper.IsEqualAsData(oldContent, content);
            if (isEqual)
            {
                return new FileAccessWriteResult {TfsVersionAndNewContentIsSameSoNothingDoneAnything = true};
            }

            if (!File.Exists(path))
            {
                File.WriteAllText(path, content);
                return new FileAccessWriteResult {ThereIsNoFileAndFileCreated = true};
            }

            var result = new FileAccessWriteResult();

            var errorMessage = TFSAccessForBOA.CheckoutFile(path);
            if (errorMessage.HasValue())
            {
                result.CheckoutError = errorMessage;
            }

            if (new FileInfo(path).IsReadOnly)
            {
                File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
                result.FileReadOnlyAttributeRemoved = true;
            }

            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }
        #endregion

        #region Methods
        static string GetTfsPath(string filePath)
        {
            return "$" + filePath.RemoveFromStart(@"d:\work",StringComparison.OrdinalIgnoreCase).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        #endregion
    }
}