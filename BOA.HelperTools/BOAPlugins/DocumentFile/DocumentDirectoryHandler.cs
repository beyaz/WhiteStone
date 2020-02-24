using System;
using System.IO;
using System.Linq;
using BOAPlugins.VSIntegration;

namespace BOAPlugins.DocumentFile
{
    class DocumentDirectoryHandler
    {
        #region Public Properties
        public string             DirectoryName { get; set; }
        public IVisualStudioLayer VisualStudio   { get; set; }
        #endregion

        #region Public Methods


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

        #region Methods
        internal void DocumentFile(string cSharpFilePath)
        {
            VisualStudio.OpenFile(cSharpFilePath);

            VisualStudio.ExecuteCommand("BOA.DocumentActiveFile");

            VisualStudio.SaveAndCloseActiveDocument();
        }
        #endregion
    }
}