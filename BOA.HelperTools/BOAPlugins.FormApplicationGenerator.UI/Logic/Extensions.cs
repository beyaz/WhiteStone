using BOAPlugins.FormApplicationGenerator.UI;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    public static class Extensions
    {
        #region Public Methods
        public static void AutoGenerateCodesAndExportFiles(this Model model)
        {
            new FileExporter(model).ExportFiles();
        }
        #endregion
    }
}