using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.WpfControls;
using BOAPlugins.TypescriptModelGeneration;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Public Methods
        public void Next()
        {
            if (string.IsNullOrWhiteSpace(Model.TfsFolderName))
            {
                Model.ViewMessage            = "Tfs Project boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.TfsFolderNameIsEnabled = false;

            Model.SolutionInfo = SolutionInfo.CreateFrom(GetSlnFilePath(Model.TfsFolderName));

            if (Model.RequestNames == null)
            {
                Model.RequestNames = CecilHelper.GetAllRequestNames($@"d:\boa\server\bin\{Model.SolutionInfo.TypeAssemblyName}");

                Model.RequestNameIsVisible = true;

                return;
            }

            if (string.IsNullOrWhiteSpace(Model.RequestName))
            {
                Model.ViewMessage            = "Request boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.FormTypeIsVisible = true;

            Model.StartTabIsVisible = false;
            Model.DesignTabIsVisible = true;

            Model.ActionButtons = new List<ActionButtonInfo>
            {
                new ActionButtonInfo
                {
                    ActionName = nameof(Next),
                    Text       = "Kaydet"
                },
                new ActionButtonInfo
                {
                    ActionName = nameof(Next),
                    Text       = "Generate Codes"
                }
            };
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                TfsFolderNameIsEnabled = true,
                TfsFolderNames         = SM.Get<InitialConfig>().TfsFolderNames,
                StartTabIsVisible      = true,
                FormTypes              = new List<string> {"Detay", "List"},
                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Next),
                        Text       = "İleri"
                    }
                }
            };
        }
        #endregion

        #region Methods
        static string GetSlnFilePath(string tfsFolderPath)
        {
            // $/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard

            var projectName = tfsFolderPath.Split('/').Last();

            return "d:\\work" + tfsFolderPath.Replace('/', '\\').RemoveFromStart("$") + Path.DirectorySeparatorChar + projectName + ".sln";
        }
        #endregion
    }
}