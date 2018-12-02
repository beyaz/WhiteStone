using BOAPlugins.FormApplicationGenerator.UI;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    public static class Extensions
    {
        #region Public Methods
        public static void AutoGenerateCodesAndExportFiles(this MainWindowModel mainWindowModel)
        {
            new FileExporter(mainWindowModel).ExportFiles();
        }
        #endregion
    }
}