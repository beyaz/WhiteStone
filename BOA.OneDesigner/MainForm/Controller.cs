using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.WpfControls;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {


        static string GetSlnFilePath(string tfsFolderPath)
        {
            // $/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard

            var projectName = tfsFolderPath.Split('/').Last();

            return "d:\\work"+tfsFolderPath.Replace('/', '\\').RemoveFromStart("$") + Path.DirectorySeparatorChar + projectName + ".sln";
        }

        #region Public Methods
        public void Next()
        {
            if (string.IsNullOrWhiteSpace(Model.SelectedTfsFolderName))
            {
                Model.ViewMessage = "Tfs Project boş olamaz.";
                Model.ViewMessageTypeIsError = true;
               return;
            }

            Model.SelectedTfsFolderNameIsIsEnabled = false;

            Model.SlnFilePath = Model.SelectedTfsFolderName;


            Model.RequestNames =  CecilHelper.GetAllTypeNames($@"d:\boa\server\bin\{NamingInfo.GetTypeAssemblyName(Model.SlnFilePath)}");

            Model.SelectedRequestNameIsIsEnabled = true;


            
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                SelectedTfsFolderNameIsIsEnabled = true,
                TfsFolderNames    = SM.Get<InitialConfig>().TfsFolderNames,
                StartTabIsVisible = true,
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
    }
}