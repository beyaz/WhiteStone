using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGeneration;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;
using WhiteStone.UI.Container;
using WhiteStone.UI.Container.Mvc;
using Host = BOA.OneDesigner.AppModel.Host;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {

        public static void Generate(ScreenInfo screenInfo)
        {
            var tsxCode = TransactionPage.Generate(screenInfo);

            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var filePath = solutionInfo.OneProjectFolder + @"ClientApp\pages\" + screenInfo.OutputTypeScriptFileName + ".tsx";

            Util.WriteFileIfContentNotEqual(filePath, tsxCode);

            App.ShowSuccessNotification("Dosya güncellendi.@filePath: "+filePath);
        }

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        DevelopmentDatabase Database => Host.Database;
        #endregion

        #region Public Methods
        public void GenerateCodes()
        {
            if (Model.ScreenInfo.JsxModel == null)
            {
                Model.ViewMessage            = "Design boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }


            Generate(Model.ScreenInfo);
        }

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

            MessagingHelper.MessagingPropertyNames = Database.GetPropertyNames(Model.ScreenInfo.MessagingGroupName);
            if (Model.ScreenInfo.FormType == FormType.BrowsePage)
            {
                if (Model.ScreenInfo.JsxModel == null)
                {
                    Model.ScreenInfo.JsxModel = new DivAsCardContainer
                    {
                        Items = new List<BCard>
                        {
                            new BCard
                            {
                                TitleInfo = new LabelInfo {IsFreeText = true, FreeTextValue = "Kriterler"}
                            },
                            new BCard
                            {
                               
                                TitleInfo = new LabelInfo {IsFreeText = true, FreeTextValue = "Sonuç Listesi"},
                                Items = new List<BField>
                                {
                                    new BDataGrid
                                    {
                                        SizeInfo = new SizeInfo{IsLarge = true}
                                    }
                                }
                            }
                        }
                    };
                }
            }
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ScreenInfo          = new ScreenInfo(),
                TfsFolderNames      = Database.GetTfsFolderNames(),
                SearchIsVisible     = true,
                FormTypes           = FormType.GetAll(),
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
            if (Model.ScreenInfoGottenFromCache )
            {
                return;
            }
            var exist = Database.Load(Model.ScreenInfo);
            if (exist)
            {
                Model.SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(Model.ScreenInfo.TfsFolderName);
            }

            if (Model.SolutionInfo != null)
            {
                Host.RequestIntellisenseData = CecilHelper.GetRequestIntellisenseData(Model.SolutionInfo.TypeAssemblyPathInServerBin, Model.ScreenInfo.RequestName);
            }

            UpdateResourceActions();
        }

        public void ResourceCodeChanged()
        {
            var exist = Database.Load(Model.ScreenInfo);
            if (exist)
            {
                Model.ScreenInfoGottenFromCache = true;
                Model.SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(Model.ScreenInfo.TfsFolderName);
            }

            if (Model.SolutionInfo != null)
            {
                Host.RequestIntellisenseData = CecilHelper.GetRequestIntellisenseData(Model.SolutionInfo.TypeAssemblyPathInServerBin, Model.ScreenInfo.RequestName);
            }

            UpdateResourceActions();

        }

        void UpdateResourceActions()
        {

            FileNamingHelper.InitDefaultOutputTypeScriptFileName(Model.ScreenInfo);

            var resourceCode = Model.ScreenInfo.ResourceCode;
            if (string.IsNullOrWhiteSpace(resourceCode))
            {
                return;
            }

            var resourceActions = DEV.GetResourceActions(resourceCode);
            if (resourceActions.Count == 0)
            {
                return;
            }


            if (Model.ScreenInfo.ResourceActions == null)
            {
                Model.ScreenInfo.ResourceActions = resourceActions;
                return;
            }
            
            // merge
            foreach (var resourceAction in Model.ScreenInfo.ResourceActions)
            {
                var existingRecord = resourceActions.FirstOrDefault(x => x.CommandName == resourceAction.CommandName);
                if (existingRecord == null)
                {
                    continue;
                }
                resourceAction.CopyTo(existingRecord);
            }

            Model.ScreenInfo.ResourceActions = resourceActions;

        }

        static readonly DevelopmentDatabase DEV = new DevelopmentDatabase();

        public void Save()
        {
            if (Model.ScreenInfo.JsxModel == null)
            {
                Model.ViewMessage            = "Design boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Database.Save(Model.ScreenInfo);

            Model.ViewMessage = "Kaydedildi.";
        }

        public void TfsFolderNameChanged()
        {
            if (Model.ScreenInfoGottenFromCache )
            {
                return;
            }

            if (Model.ScreenInfo.TfsFolderName != null)
            {
                Model.SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(Model.ScreenInfo.TfsFolderName);
                Model.RequestNames = CecilHelper.GetAllRequestNames(Model.SolutionInfo.TypeAssemblyPathInServerBin);
            }
        }
        #endregion
    }
}