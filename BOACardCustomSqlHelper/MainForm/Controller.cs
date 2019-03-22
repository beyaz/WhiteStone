using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGeneration;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;
using WhiteStone.UI.Container;
using WhiteStone.UI.Container.Mvc;
using Host = BOA.OneDesigner.AppModel.Host;

namespace BOACardCustomSqlHelper.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public static void Generate(ScreenInfo screenInfo)
        {
            var tsxCode = TransactionPage.Generate(screenInfo);

            tsxCode = TsxCodeBeautifier.Beautify(tsxCode);

            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var filePath = solutionInfo.OneProjectFolder + @"ClientApp\pages\" + screenInfo.OutputTypeScriptFileName + ".tsx";

            Util.WriteFileIfContentNotEqual(filePath, tsxCode);

            App.ShowSuccessNotification("Dosya güncellendi.@filePath: " + filePath);
        }

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

            using (var database = new DevelopmentDatabase())
            {
                MessagingHelper.MessagingPropertyNames = database.GetPropertyNames(Model.ScreenInfo.MessagingGroupName);
            }

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
                                IsBrowsePageCriteria = true,
                                TitleInfo            = new LabelInfo {IsFreeText = true, FreeTextValue = "Kriterler"}
                            },
                            new BCard
                            {
                                IsBrowsePageDataGridContainer = true,
                                LayoutProps                   = new LayoutProps {Wide     = 12},
                                TitleInfo                     = new LabelInfo {IsFreeText = true, FreeTextValue = "Sonuç Listesi"},
                                Items = new List<BField>
                                {
                                    new BDataGrid
                                    {
                                        SizeInfo = new SizeInfo {IsLarge = true}
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
                ScreenInfo = new ScreenInfo(),

                SearchIsVisible = true,
                FormTypes       = FormType.GetAll(),

                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Next),
                        Text       = "İleri"
                    }
                }
            };

            using (var database = new DevelopmentDatabase())
            {
                Model.TfsFolderNames      = database.GetTfsFolderNames();
                Model.MessagingGroupNames = database.GetMessagingGroupNames();
                Model.RequestNames        = database.GetDefaultRequestNames();
            }
        }

        public void RequestNameChanged()
        {
            TryToInitializeFromDb();
        }

        public void ResourceCodeChanged()
        {
            TryToInitializeFromDb();
        }

        public void Save()
        {
            if (Model.ScreenInfo.JsxModel == null)
            {
                Model.ViewMessage            = "Design boş olamaz.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            using (var database = new DevelopmentDatabase())
            {
                database.Save(Model.ScreenInfo);
            }

            Model.ViewMessage = "Kaydedildi.";
        }

        public void TfsFolderNameChanged()
        {
            if (Model.ScreenInfo.TfsFolderName.HasValue())
            {
                Model.SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(Model.ScreenInfo.TfsFolderName);
                Model.RequestNames = CecilHelper.GetAllRequestNames(Model.SolutionInfo.TypeAssemblyPathInServerBin);
            }
        }

        public void TryToInitializeFromDb()
        {
            var screenInfo = Model.ScreenInfo;

            screenInfo.ResourceCode = screenInfo.ResourceCode?.Trim();

            var exist = false;
            using (var database = new DevelopmentDatabase())
            {
                exist = database.Load(screenInfo);
            }

            if (exist)
            {
                Model.SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);
            }

            if (Model.SolutionInfo != null)
            {
                Host.RequestIntellisenseData = CecilHelper.GetRequestIntellisenseData(Model.SolutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName);
                if (Host.RequestIntellisenseData == null)
                {
                    Model.ViewMessage            = Error.GetMessageRequestNotFound(screenInfo.RequestName, Model.SolutionInfo.TypeAssemblyPathInServerBin);
                    Model.ViewMessageTypeIsError = true;
                }
            }

            UpdateResourceActions();
        }
        #endregion

        #region Methods
        void UpdateResourceActions()
        {
            FileNamingHelper.InitDefaultOutputTypeScriptFileName(Model.ScreenInfo);

            var resourceCode = Model.ScreenInfo.ResourceCode;
            if (string.IsNullOrWhiteSpace(resourceCode))
            {
                return;
            }

            List<Aut_ResourceAction> resourceActions = null;

            using (var database = new DevelopmentDatabase())
            {
                resourceActions = database.GetResourceActions(resourceCode);
            }

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
        #endregion
    }
}