using System;
using System.IO;
using System.Linq;
using BOAPlugins.VSIntegration;

namespace BOAPlugins.DocumentFile
{
    /// <summary>
    ///     The document directory handler
    /// </summary>
    class DocumentDirectoryHandler
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the directory.
        /// </summary>
        public string DirectoryName { get; set; }

        /// <summary>
        ///     Gets or sets the visual studio.
        /// </summary>
        public IVisualStudioLayer VisualStudio { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public void Execute()
        {
            if (DirectoryName == null)
            {
                throw new ArgumentNullException(nameof(DirectoryName));
            }

            var cSharpFiles = Directory.GetFiles(DirectoryName, "*.cs", SearchOption.AllDirectories);

            cSharpFiles = cSharpFiles.Where(FileCanBeDocument).ToArray();

            foreach (var cSharpFile in cSharpFiles)
            {
                DocumentFile(cSharpFile);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Documents the file.
        /// </summary>
        internal void DocumentFile(string cSharpFilePath)
        {
            VisualStudio.OpenFile(cSharpFilePath);

            VisualStudio.ExecuteCommand("BOA.DocumentActiveFile");

            VisualStudio.SaveAndCloseActiveDocument();
        }

        /// <summary>
        ///     Files the can be document.
        /// </summary>
        static bool FileCanBeDocument(string path)
        {
            if (path.Contains(@"\obj\"))
            {
                return false;
            }

            if (path.Contains(@"\bin\"))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}