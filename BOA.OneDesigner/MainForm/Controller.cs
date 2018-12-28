using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using BOA.UI.Types;
using BOAPlugins.TypescriptModelGeneration;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {

        IDatabase Database => UIContext.Database;

        #region Public Methods
        public void Next()
        {
            if (string.IsNullOrWhiteSpace(Model.ScreenInfo.TfsFolderName))
            {
                Model.ViewMessage            = "Tfs Project boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

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

            if (string.IsNullOrWhiteSpace(Model.ScreenInfo.MessagingGroupName))
            {
                Model.ViewMessage            = "Messaging Group boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.SearchIsVisible = false;

            Model.DesignIsVisible = true;

            Model.ActionButtons = new List<ActionButtonInfo>
            {
                new ActionButtonInfo
                {
                    ActionName = nameof(Save),
                    Text       = "Kaydet"
                },
                new ActionButtonInfo
                {
                    ActionName = nameof(GenerateCodes),
                    Text       = "Generate Codes"
                }
            };

            UIContext.MessagingPropertyNames = BOAPlugins.Messaging.DataSource.GetPropertyNames(Model.ScreenInfo.MessagingGroupName);


        }

        public void Save()
        {

            Database.Save(Model.ScreenInfo);

            Model.ViewMessage = "Kaydedildi.";


        }
        public void GenerateCodes()
        {

        }
        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ScreenInfo      = new ScreenInfo(),
                TfsFolderNames  = Database.GetTfsFolderNames(),
                SearchIsVisible = true,
                FormTypes       = new List<string> {"Detay", "List"},
                MessagingGroupNames = Database.GetMessagingGroupNames(),
               
                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Next),
                        Text       = "İleri"
                    }
                },

                RequestNames = Database.GetDefaultRequestNames()
            };
        }

        public void RequestNameChanged()
        {

            var screenInfo = Database.GetScreenInfo(Model.ScreenInfo.RequestName);
            if (screenInfo != null)
            {
                Model.ScreenInfo = screenInfo;
                Model.SolutionInfo = SolutionInfo.CreateFrom(GetSlnFilePath(Model.ScreenInfo.TfsFolderName));
            }

            if (Model.SolutionInfo != null)
            {
                UIContext.RequestPropertyIntellisense = CecilHelper.GetAllBindProperties(Model.SolutionInfo.TypeAssemblyPathInServerBin, Model.ScreenInfo.RequestName);    
            }
            
        }

        public void TfsFolderNameChanged()
        {
            if (Model.ScreenInfo.TfsFolderName != null)
            {
                Model.SolutionInfo = SolutionInfo.CreateFrom(GetSlnFilePath(Model.ScreenInfo.TfsFolderName));
                Model.RequestNames = CecilHelper.GetAllRequestNames(Model.SolutionInfo.TypeAssemblyPathInServerBin);
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