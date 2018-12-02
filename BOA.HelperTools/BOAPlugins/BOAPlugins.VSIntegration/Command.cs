using System.IO;
using BOAPlugins.Messaging;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOAPlugins.VSIntegration
{
    class Command
    {
        #region Public Properties
        public IVisualStudioLayer VisualStudio { get; set; }
        #endregion

        #region Public Methods
        public void RemoveUnusedMessagesInCsCodes()
        {
            var data = new MessagingExporterData
            {
                SolutionFilePath       = VisualStudio.GetSolutionFilePath(),
                RemoveUnusedProperties = true
            };

            MessagingExporter.ExportAsCSharpCode(data);
            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusbarText(data.ErrorMessage);
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            VisualStudio.UpdateStatusbarText(Path.GetFileName(data.TargetFilePath) + " successfully updated.");
        }

        public void RemoveUnusedMessagesInTypescriptCodes()
        {
            var solutionFilePath = VisualStudio.GetSolutionFilePath();

            var data = new MessagingExporterData
            {
                SolutionFilePath       = solutionFilePath,
                RemoveUnusedProperties = true
            };
            MessagingExporter.ExportAsTypeScriptCode(data);
            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusbarText(data.ErrorMessage);
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            VisualStudio.UpdateStatusbarText("Messages.tsx successfully updated.");
        }

        public void UpdateMessageCs()
        {
            var data = new MessagingExporterData
            {
                SolutionFilePath = VisualStudio.GetSolutionFilePath()
            };

            MessagingExporter.ExportAsCSharpCode(data);
            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusbarText(data.ErrorMessage);
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            VisualStudio.UpdateStatusbarText(Path.GetFileName(data.TargetFilePath) + " successfully updated.");
        }

        public void UpdateMessageTsx()
        {
            var data = new MessagingExporterData
            {
                SolutionFilePath = VisualStudio.GetSolutionFilePath()
            };

            MessagingExporter.ExportAsTypeScriptCode(data);
            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusbarText(data.ErrorMessage);
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            VisualStudio.UpdateStatusbarText("Messages.tsx successfully updated.");
        }

        public void UpdateTypeScriptModels()
        {
            var solutionFilePath = VisualStudio.GetSolutionFilePath();

            var data = Handler.Handle(solutionFilePath);

            if (data.ErrorMessage != null)
            {
                VisualStudio.UpdateStatusbarText(data.ErrorMessage);
                return;
            }

            VisualStudio.UpdateStatusbarText(data.InfoMessage);
        }
        #endregion
    }
}