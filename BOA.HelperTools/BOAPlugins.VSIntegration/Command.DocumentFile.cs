using BOAPlugins.DocumentFile;
using System;
using EnvDTE;
using EnvDTE80;

namespace BOASpSearch
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed partial class Command1
    {
        #region Methods

        void xx()
        {
            var path = @"D:\git\PropertyMapper\PropertyMapper\Infrastructure\GenericProxy\IProxyInput.cs";

            VisualStudio.OpenFile(path);

            DocumentFile(null,null);

            var dte = (DTE2) ServiceProvider.GetService(typeof(DTE));
            
            dte.ActiveDocument.Close(vsSaveChanges.vsSaveChangesYes);
               
        }

        void DocumentFile(object sender, EventArgs e)
        {
            xx();
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