using System.Windows;

namespace BOAPlugins.VSIntegration
{
    /// <summary>
    ///     The visual studio layer
    /// </summary>
    public interface IVisualStudioLayer
    {
        #region Public Properties
        /// <summary>
        ///     Gets the active project csproj file path.
        /// </summary>
        string ActiveProjectCsprojFilePath { get; }

        /// <summary>
        ///     Gets the name of the active project.
        /// </summary>
        string ActiveProjectName { get; }

        /// <summary>
        ///     Gets the cursor selected text.
        /// </summary>
        string CursorSelectedText { get; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Actives the document replace text.
        /// </summary>
        void ActiveDocument_ReplaceText(string text, string newText);

        /// <summary>
        ///     Actives the document save.
        /// </summary>
        bool ActiveDocument_Save();

        /// <summary>
        ///     Actives the document select all.
        /// </summary>
        void ActiveDocument_SelectAll();

        /// <summary>
        ///     Creates the new SQL file.
        /// </summary>
        void CreateNewSQLFile(string text, string name);

        /// <summary>
        ///     Documents the active file.
        /// </summary>
        void DocumentActiveFile();

        /// <summary>
        ///     Gets the bin folder path of active project.
        /// </summary>
        string GetBinFolderPathOfActiveProject();

        /// <summary>
        ///     Gets the solution file path.
        /// </summary>
        string GetSolutionFilePath();

        /// <summary>
        ///     Opens the file.
        /// </summary>
        void OpenFile(string filePath);

        /// <summary>
        ///     Shows the dialog.
        /// </summary>
        void ShowDialog(Window window);

        /// <summary>
        ///     Updates the status bar text.
        /// </summary>
        void UpdateStatusBarText(string message);

        /// <summary>
        ///     Updates the status bar text.
        /// </summary>
        void UpdateStatusBarText(string message, int timeout);
        #endregion
    }
}