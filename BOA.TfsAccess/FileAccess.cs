using System;
using System.IO;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;

namespace BOA.TfsAccess
{
    /// <summary>
    ///     The file access
    /// </summary>
    public class FileAccess
    {
        #region Public Methods
        /// <summary>
        ///     Writes all text.
        /// </summary>
        public virtual FileAccessWriteResult WriteAllText(string path, string content)
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
                WriteToFileSystem(path,content,new FileAccessWriteResult());

                return new FileAccessWriteResult {TfsVersionAndNewContentIsSameSoNothingDoneAnything = true};
            }

            if (!File.Exists(path))
            {
                FileHelper.WriteAllText(path, content);
                return new FileAccessWriteResult {ThereIsNoFileAndFileCreated = true};
            }

            var result = new FileAccessWriteResult();

            var errorMessage = TFSAccessForBOA.CheckoutFile(path);
            if (errorMessage.HasValue())
            {
                result.CheckoutError = errorMessage;
            }
            else
            {
                result.FileIsCheckOutFromTfs = true;
            }

            WriteToFileSystem(path,content,result);
            
            return result;
        }

        /// <summary>
        ///     Writes to file system.
        /// </summary>
        public static void WriteToFileSystem(string path, string content, FileAccessWriteResult result)
        {
            try
            {
                if (new FileInfo(path).Exists && new FileInfo(path).IsReadOnly)
                {
                    File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
                    result.FileReadOnlyAttributeRemoved = true;
                }

                FileHelper.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        ///     Gets the TFS path.
        /// </summary>
        static string GetTfsPath(string filePath)
        {
            return "$" + filePath.RemoveFromStart(@"d:\work",StringComparison.OrdinalIgnoreCase).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        #endregion
    }

    
}