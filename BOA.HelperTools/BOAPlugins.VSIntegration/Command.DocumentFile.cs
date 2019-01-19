using BOAPlugins.DocumentFile;
using System;

namespace BOASpSearch
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed partial class Command1
    {
        #region Methods

        void DocumentFile(object sender, EventArgs e)
        {
            try
            {
                VisualStudio.DocumentActiveFile();
            }
            catch (Exception)
            {
            }

            VisualStudio.ActiveDocument_SelectAll();

            var selectedText = VisualStudio.CursorSelectedText;

            var handler = new Handler();

            var data = new Data
            {
                CSharpCode = selectedText
            };

            try
            {
                handler.Handle(data);
            }
            catch (Exception exception)
            {
                VisualStudio.UpdateStatusBarText(exception.Message);
                return;
            }

            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusBarText(data.ErrorMessage);
                return;
            }

            VisualStudio.ActiveDocument_ReplaceText(selectedText, data.CSharpCode);

            VisualStudio.ActiveDocument_Save();

            VisualStudio.UpdateStatusBarText("Successfully documented.");
        }

        #endregion Methods
    }
}