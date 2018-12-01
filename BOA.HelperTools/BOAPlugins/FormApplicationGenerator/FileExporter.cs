﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.ExportingModel;
using BOAPlugins.FormApplicationGenerator.Types;

namespace BOAPlugins.FormApplicationGenerator
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
            var typesFolder = Model.TypesProjectFolder;

            FixData(Model);

            var oneProjectFolder = Model.OneProjectFolder;

            Util.WriteFileIfContentNotEqual(typesFolder + Model.FormName + "ListForm.cs", TypeFileForListForm.GenerateCode(Model));
            Util.WriteFileIfContentNotEqual(typesFolder + Model.FormName + "Form.cs", TypeFileForDefinitionForm.GenerateCode(Model));

            Util.WriteFileIfContentNotEqual(Model.OrchestrationProjectFolder + Model.FormName + "ListForm.cs", OrchestrationFileForListForm.GenerateCode(Model));
            Util.WriteFileIfContentNotEqual(Model.OrchestrationProjectFolder + Model.FormName + "Form.cs", OrchestrationFileForDefinitionForm.GenerateCode(Model));

            FormAssistantProjectInitializer.Initialize(Model);

            Util.WriteFileIfContentNotEqual(oneProjectFolder + @"ClientApp\pages\" + Model.FormName + "ListForm.tsx", Model.ToBrowsePageTemplate().TransformText());
            Util.WriteFileIfContentNotEqual(oneProjectFolder + @"ClientApp\pages\" + Model.FormName + "Form.tsx", Model.ToTransactionPageTemplate().TransformText());

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
            var config = JsonHelper.Deserialize<ExportContract>(File.ReadAllText(autoGeneratedConfigFilePath));

            var assemblyName = Model.NamespaceNameForType + ".dll";
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

            var ns = Model.NamespaceNameForType;

            var ExportClassNames = exportInfo.ExportClassNames;

            TryAddIfNotExists(ExportClassNames, ns + "." + Model.DefinitionFormDataClassName);
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.FormName + "FormDataSource");
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.FormName + "FormRequest");

            TryAddIfNotExists(ExportClassNames, ns + "." + Model.FormName + "ListFormDataSource");
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.FormName + "ListFormRequest");
            TryAddIfNotExists(ExportClassNames, ns + "." + Model.FormName + "ListFormData");

            Util.WriteFileIfContentNotEqual(autoGeneratedConfigFilePath, JsonHelper.Serialize(config));
        }
        #endregion
    }
}