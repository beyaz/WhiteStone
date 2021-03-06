﻿using BOAPlugins.Utility;

namespace BOAPlugins.TypescriptModelGeneration
{
    public static class Handler
    {
        #region Public Methods
        public static ExportData Handle(string solutionFilePath)
        {
            var solutionInfo = GenerateFilePathInfo.CreateFrom(solutionFilePath);
            if (solutionInfo.AutoGeneratedModelsConfig_JsonFilePath == null)
            {
                return new ExportData
                {
                    ErrorMessage = nameof(solutionInfo.AutoGeneratedModelsConfig_JsonFilePath) + " not found."
                };
            }

            var configFilePath = solutionInfo.AutoGeneratedModelsConfig_JsonFilePath;

            var data = Exporter.Export(configFilePath);

            Util.WriteFileIfContentNotEqual(solutionInfo.AutoGeneratedModels_tsx_FilePath, data.GeneratedTSCode);

            Util.WriteFileIfContentNotEqual(solutionInfo.AutoGenerated_tsx_FilePath, data.AutoGeneratedCodesInUtilDirectory);

            data.InfoMessage = "TypeScript autogenerated codes successfully updated.";

            return data;
        }
        #endregion
    }
}