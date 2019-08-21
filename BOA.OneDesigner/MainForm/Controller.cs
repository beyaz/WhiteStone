using System.Collections.Generic;
using System.Windows;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGeneration;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.Deployment;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.TfsAccess;
using BOAPlugins.TypescriptModelGeneration;
using WhiteStone.UI.Container;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    /// <summary>
    /// The controller
    /// </summary>
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

            var filePath = GetOutputFilePath(screenInfo);

            var message = "Dosya güncellendi. @filePath: " + filePath;

            var result = new FileAccess().WriteAllText(filePath, tsxCode);
            if (result.TfsVersionAndNewContentIsSameSoNothingDoneAnything)
            {
                App.ShowSuccessNotification($"Dosya zaten güncel. @filePath:{filePath}");
                return;
            }

            if (result.ThereIsNoFileAndFileCreated)
            {
                App.ShowSuccessNotification($"Dosya oluşturuldu. @filePath:{filePath}");
                return;
            }

            if (result.Exception != null)
            {
                App.ShowErrorNotification(result.Exception.Message);
                return;
            }

            App.ShowSuccessNotification($"Dosya güncellendi. @filePath:{filePath}");
        }

        public static string GetOutputFilePath(ScreenInfo screenInfo)
        {
            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            return solutionInfo.OneProjectFolder + @"ClientApp\pages\" + screenInfo.OutputTypeScriptFileName + ".tsx";
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
            else
            {
                // Mayber converted browsepage to  transaction page
                if (Model.ScreenInfo.JsxModel is DivAsCardContainer divAsCardContainer)
                {
                    foreach (var card in divAsCardContainer.Items)
                    {
                        card.IsBrowsePageCriteria          = false;
                        card.IsBrowsePageDataGridContainer = false;
                    }
                }
            }

            using (var database = new DevelopmentDatabase())
            {
                database.CommandText = $"SELECT TOP 1 Name FROM AUT.Resource WITH(NOLOCK) WHERE ResourceCode = '{Model.ScreenInfo.ResourceCode}'";
                var name = database.ExecuteScalar();

                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Title = Model.ScreenInfo.ResourceCode + " - " + name;
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

            Updater.StartUpdate();
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

            Model.ScreenInfo.ResourceActions = ResourceActionHelper.GetResourceActions(Model.ScreenInfo.ResourceActions, resourceCode);
        }
        #endregion
    }
}