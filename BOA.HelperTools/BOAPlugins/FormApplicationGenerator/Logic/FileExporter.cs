﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.FormApplicationGenerator.UI;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    class FileExporter
    {
        #region Constructors
        public FileExporter(Model model)
        {
            Model = model;
        }
        #endregion

        #region Properties
        Model Model { get; }
        #endregion

        #region Public Methods

        static void FixData(Model model)
        {
            if (model.ListFormSearchFields == null)
            {
                model.ListFormSearchFields = new List<BField>();
            }

            foreach (var bField in model.ListFormSearchFields)
            {
                if (bField.Label == null)
                {
                    if (model.UserRawStringForMessaging)
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
            FixData(Model);

            var oneProjectFolder = Model.SolutionInfo.OneProjectFolder;

            //Util.WriteFileIfContentNotEqual(typesFolder + Model.FormName + "ListForm.cs", TypeFileForListForm.GenerateCode(Model));
            //Util.WriteFileIfContentNotEqual(typesFolder + Model.FormName + "Form.cs", TypeFileForDefinitionForm.GenerateCode(Model));

            Util.WriteFileIfContentNotEqual(Model.OrchestrationProjectFolder + Model.TableNameInDatabase + "ListForm.cs", Model.ToOrchestrationFileForListForm().TransformText());
            Util.WriteFileIfContentNotEqual(Model.OrchestrationProjectFolder + Model.TableNameInDatabase + "Form.cs", Model.ToOrchestrationFileForDefinitionForm().TransformText());

            FormAssistantProjectInitializer.Initialize(Model);

            Util.WriteFileIfContentNotEqual(oneProjectFolder + @"ClientApp\pages\" + Model.TableNameInDatabase + "ListForm.tsx", Model.ToBrowsePageTemplate().TransformText());
            Util.WriteFileIfContentNotEqual(oneProjectFolder + @"ClientApp\pages\" + Model.TableNameInDatabase + "Form.tsx", Model.ToTransactionPageTemplate().TransformText());

            var autoGeneratedConfigFilePath = oneProjectFolder + @"ClientApp\models\AutoGeneratedModelsConfig.json";

            UpdateAutoGeneratedModelsConfig(autoGeneratedConfigFilePath);
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

            var assemblyName = Model.NamingInfo.TypeAssemblyName;
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

            var ns = Model.NamingInfo.NamespaceNameForType;

            var ExportClassNames = exportInfo.ExportClassNames;

            TryAddIfNotExists(ExportClassNames, ns + "." + Model.NamingInfo.DefinitionFormDataClassName);
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.TableNameInDatabase + "FormDataSource");
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.TableNameInDatabase + "FormRequest");

            TryAddIfNotExists(ExportClassNames, ns + "." + Model.TableNameInDatabase + "ListFormDataSource");
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.TableNameInDatabase + "ListFormRequest");
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.TableNameInDatabase + "ListFormData");

            Util.WriteFileIfContentNotEqual(autoGeneratedConfigFilePath, JsonHelper.Serialize(config));
        }
        #endregion
    }
}