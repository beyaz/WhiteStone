using System;
using BOAPlugins.DocumentFile;

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
            VisualStudio.DocumentActiveFile();
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
                VisualStudio.UpdateStatusbarText(exception.Message);
                return;
            }

            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusbarText(data.ErrorMessage);
                return;
            }

            VisualStudio.ActiveDocument_ReplaceText(selectedText, data.CSharpCode);

            VisualStudio.ActiveDocument_Save();

            VisualStudio.UpdateStatusbarText("Successfully documented.");
        }
        #endregion
    }
}