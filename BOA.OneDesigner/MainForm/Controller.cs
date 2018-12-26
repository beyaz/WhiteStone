using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
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
            if (string.IsNullOrWhiteSpace(Model.ScreenInfo.TfsFolderName))
            {
                Model.ViewMessage            = "Tfs Project boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.SolutionInfo = SolutionInfo.CreateFrom(GetSlnFilePath(Model.ScreenInfo.TfsFolderName));

            Model.RequestNames = CecilHelper.GetAllRequestNames($@"d:\boa\server\bin\{Model.SolutionInfo.TypeAssemblyName}");


            if (string.IsNullOrWhiteSpace(Model.ScreenInfo.RequestName))
            {
                Model.ViewMessage            = "Request boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(Model.ScreenInfo.FormType))
            {
                Model.ViewMessage            = "Form Tipi boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.SearchIsVisible = false;

            Model.DesignIsVisible = true;

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


            var screenInfo = SM.Get<InitialConfig>().ScreenInfoList?.FirstOrDefault(x => x.RequestName == Model.ScreenInfo.RequestName);
            if (screenInfo == null)
            {
                SM.Get<InitialConfig>().ScreenInfoList.Add(Model.ScreenInfo);

                InitialConfigCache.Save();
            }


        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ScreenInfo        = new ScreenInfo(),
                TfsFolderNames    = SM.Get<InitialConfig>().TfsFolderNames,
                SearchIsVisible = true,
                FormTypes         = new List<string> {"Detay", "List"},
                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Next),
                        Text       = "İleri"
                    }
                },

                RequestNames = SM.Get<InitialConfig>().ScreenInfoList?.Select(x => x.RequestName).ToList()
            };
        }

        public void RequestNameChanged()
        {
            var screenInfo = SM.Get<InitialConfig>().ScreenInfoList?.FirstOrDefault(x => x.RequestName == Model.ScreenInfo.RequestName);
            if (screenInfo != null)
            {
                Model.ScreenInfo = screenInfo;
            }
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