﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.FormApplicationGenerator.UI;
using BOAPlugins.Utility;
using BOAPlugins.Utility.TypescriptModelGeneration;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    class FileExporter
    {
        #region Constructors
        public FileExporter(MainWindowModel mainWindowModel)
        {
            MainWindowModel = mainWindowModel;
        }
        #endregion

        #region Properties
        MainWindowModel MainWindowModel { get; }
        #endregion

        #region Public Methods

        static void FixData(MainWindowModel mainWindowModel)
        {
            if (mainWindowModel.ListFormSearchFields == null)
            {
                mainWindowModel.ListFormSearchFields = new List<BField>();
            }

            foreach (var bField in mainWindowModel.ListFormSearchFields)
            {
                if (bField.Label == null)
                {
                    if (mainWindowModel.UserRawStringForMessaging)
                    {
                        bField.Label = '"' + bField.Name + '"';
                    }
                    else
                    {
                        bField.Label = "Message." + bField.Name;
                    }
                }
            }
        }


        public void ExportFiles()
        {
            FixData(MainWindowModel);


            Util.WriteFileIfContentNotEqual(MainWindowModel.SolutionInfo.OrchestrationProjectFolder + MainWindowModel.NamingInfo.OrchestrationFileNameForListForm, MainWindowModel.ToOrchestrationFileForListForm().TransformText());
            Util.WriteFileIfContentNotEqual(MainWindowModel.SolutionInfo.OrchestrationProjectFolder + MainWindowModel.NamingInfo.OrchestrationFileNameForDetailForm, MainWindowModel.ToOrchestrationFileForDefinitionForm().TransformText());

            FormAssistantProjectInitializer.Initialize(MainWindowModel.SolutionInfo);

            Util.WriteFileIfContentNotEqual(MainWindowModel.TsxFilePathOfListForm, MainWindowModel.ToBrowsePageTemplate().TransformText());
            Util.WriteFileIfContentNotEqual(MainWindowModel.TsxFilePathOfDetailForm, MainWindowModel.ToTransactionPageTemplate().TransformText());

            UpdateAutoGeneratedModelsConfig(MainWindowModel.SolutionInfo.AutoGeneratedModelsConfig_JsonFilePath);
        }
        #endregion

        #region Methods
        static string CannotBeEmpty(string labelName)
        {
            return labelName + " can not be empty";
        }

        static void TryAddIfNotExists(ICollection<string> ExportClassNames, string fullTypeName)
        {
            if (ExportClassNames.Contains(fullTypeName))
            {
                return;
            }

            ExportClassNames.Add(fullTypeName);
        }

        void UpdateAutoGeneratedModelsConfig(string autoGeneratedConfigFilePath)
        {
            var config = JsonHelper.Deserialize<ExportData>(File.ReadAllText(autoGeneratedConfigFilePath));

            var assemblyName = MainWindowModel.NamingInfo.TypeAssemblyName;
            var exportInfo   = config.ExportInfoList.FirstOrDefault(x => x.Assembly == assemblyName);
            if (exportInfo == null)
            {
                exportInfo = new ExportInfo
                {
                    Assembly         = assemblyName,
                    ExportClassNames = new List<string>()
                };

                config.ExportInfoList.Add(exportInfo);
            }

            var ns = MainWindowModel.NamingInfo.NamespaceNameForType;

            var ExportClassNames = exportInfo.ExportClassNames;

            TryAddIfNotExists(ExportClassNames, ns + "." + MainWindowModel.NamingInfo.DefinitionFormDataClassName);
            TryAddIfNotExists(ExportClassNames, ns + "." + MainWindowModel.TableNameInDatabase + "FormDataSource");
            TryAddIfNotExists(ExportClassNames, ns + "." + MainWindowModel.TableNameInDatabase + "FormRequest");

            TryAddIfNotExists(ExportClassNames, ns + "." + MainWindowModel.TableNameInDatabase + "ListFormDataSource");
            TryAddIfNotExists(ExportClassNames, ns + "." + MainWindowModel.TableNameInDatabase + "ListFormRequest");
            TryAddIfNotExists(ExportClassNames, ns + "." + MainWindowModel.TableNameInDatabase + "ListFormData");

            Util.WriteFileIfContentNotEqual(autoGeneratedConfigFilePath, JsonHelper.Serialize(config));
        }
        #endregion
    }
}