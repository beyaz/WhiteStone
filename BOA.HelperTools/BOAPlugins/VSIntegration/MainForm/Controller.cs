﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BOA.CodeGeneration.Util;
using BOAPlugins.Messaging;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;
using BOAPlugins.ViewClassDependency;
using WhiteStone;
using WhiteStone.UI.Container.Mvc;
using Handler = BOAPlugins.TypescriptModelGeneration.Handler;

namespace BOAPlugins.VSIntegration.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Properties
        IVisualStudioLayer VisualStudio => SM.Get<IVisualStudioLayer>();
        #endregion

        #region Public Methods
        public void CheckInSolution()
        {
            var data = new CheckInSolutionInput
            {
                SolutionFilePath = Model.SolutionFilePath,
                Comment          = Model.SolutionCheckInComment
            };

            TFSAccessForBOA.CheckInSolution(data);

            Model.ViewMessage = data.ResultMessage;

            Model.ViewShouldBeClose = true;
        }

        public void InitializeFormAssistantDefaultCodes()
        {
            var solutionInfo = SolutionInfo.CreateFrom(Model.SolutionFilePath);

            FormAssistantProjectInitializer.Initialize(solutionInfo);

            Model.ViewShouldBeClose = true;
            Model.ViewMessage       = "Files are exported.";
        }

        public override void OnViewClose()
        {
            var configuration = SM.Get<Configuration>();

            configuration.SolutionCheckInComments.SetValue(Model.SolutionFilePath,Model.SolutionCheckInComment);

            Configuration.SaveToFile();
        }

        public override void OnViewLoaded()
        {
            var configuration = SM.Get<Configuration>();

            

            Model = new Model
            {
                CursorSelectedText     = VisualStudio.CursorSelectedText,
                SolutionFilePath       = VisualStudio.GetSolutionFilePath(),
                CheckInSolutionIsEnabled = configuration.CheckInSolutionIsEnabled||Environment.UserName == "beyaztas"
            };

            Model.SolutionCheckInComment = configuration.SolutionCheckInComments.TryGetValue(Model.SolutionFilePath,null);
            

            if (string.IsNullOrWhiteSpace(Model.CursorSelectedText))
            {
                Model.MethodCallGraphIsVisible = false;
            }
            else
            {
                Model.MethodCallGraphIsVisible = true;

                Model.MethodCallGraphButtonText = "Call Graph -> " + Model.CursorSelectedText;
            }
        }

        public void OpenPluginDirectory()
        {
            Model.ViewShouldBeClose = true;

            Process.Start(ConstConfiguration.PluginDirectory);
        }

        public void RemoveUnusedMessagesInCsCodes()
        {
            var data = new MessagingExporterData
            {
                SolutionFilePath       = Model.SolutionFilePath,
                RemoveUnusedProperties = true
            };

            MessagingExporter.ExportAsCSharpCode(data);
            if (data.ErrorMessage != null)
            {
                Model.ViewMessage            = data.ErrorMessage;
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);

            Model.ViewMessage = Path.GetFileName(data.TargetFilePath) + " successfully updated.";

            Model.ViewShouldBeClose = true;
        }

        public void RemoveUnusedMessagesInTypescriptCodes()
        {
            var solutionFilePath = Model.SolutionFilePath;

            var data = new MessagingExporterData
            {
                SolutionFilePath       = solutionFilePath,
                RemoveUnusedProperties = true
            };
            MessagingExporter.ExportAsTypeScriptCode(data);
            if (data.ErrorMessage != null)
            {
                Model.ViewMessage            = data.ErrorMessage;
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            Model.ViewMessage = "Messages.tsx successfully updated.";
        }

        public void UpdateMessageCs()
        {
            var data = new MessagingExporterData
            {
                SolutionFilePath = Model.SolutionFilePath
            };

            MessagingExporter.ExportAsCSharpCode(data);
            if (data.ErrorMessage != null)
            {
                Model.ViewMessage            = data.ErrorMessage;
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            Model.ViewMessage = Path.GetFileName(data.TargetFilePath) + " successfully updated.";
        }

        public void UpdateMessageTsx()
        {
            var data = new MessagingExporterData
            {
                SolutionFilePath = Model.SolutionFilePath
            };

            MessagingExporter.ExportAsTypeScriptCode(data);
            if (data.ErrorMessage != null)
            {
                Model.ViewMessage            = data.ErrorMessage;
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Util.WriteFileIfContentNotEqual(data.TargetFilePath, data.GeneratedCode);
            Model.ViewMessage = "Messages.tsx successfully updated.";
        }

        public void UpdateTypeScriptModels()
        {
            var solutionFilePath = Model.SolutionFilePath;

            var data = Handler.Handle(solutionFilePath);

            if (data.ErrorMessage != null)
            {
                Model.ViewMessage            = data.ErrorMessage;
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.ViewMessage = data.InfoMessage;

            Model.ViewShouldBeClose = true;
        }

        public void ViewMethodCallGraph()
        {
            var input = new Data
            {
                AssemblySearchDirectoryPath = VisualStudio.GetBinFolderPathOfActiveProject(),
                SelectedText                = Model.CursorSelectedText,
                ActiveProjectName           = VisualStudio.ActiveProjectName
            };

            var result = new ViewClassDependency.Handler().Handle(input);

            if (result.ErrorMessage != null)
            {
                Model.ViewMessage            = result.ErrorMessage;
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.ViewShouldBeClose = true;

            VisualStudio.OpenFile(input.OutputFileFullPath);
        }
        #endregion
    }
}