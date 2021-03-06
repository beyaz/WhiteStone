﻿using System.IO;
using BOA.CodeGeneration.Util;
using BOA.TfsAccess;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOAPlugins.VSIntegration
{
    public static class FormAssistantProjectInitializer
    {
        
      

        #region Public Methods
        public static void Initialize(GenerateFilePathInfo generateFilePathInfo)
        {
            Export_FormAssistant_cs(generateFilePathInfo);
            Export_Orchestration_Extension_Class(generateFilePathInfo);
        }
        #endregion

        #region Methods
        static void Export_FormAssistant_cs(GenerateFilePathInfo generateFilePathInfo)
        {
            

            // FormAssistant.cs 
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard/BOA.Types.CardGeneral.DebitCard/FormAssistant.cs";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardGeneral.DebitCard", generateFilePathInfo.SolutionInfo.NamespaceNameForType);

            
            var targetFilePath = generateFilePathInfo.FilePathOf_FormAssistant_cs_In_Types;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        

        static void Export_Orchestration_Extension_Class(GenerateFilePathInfo generateFilePathInfo)
        {
            // Extensions.designer.cs
            const string tfsPath     = @"$/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard/BOA.Orchestration.CardGeneral.DebitCard/Extensions.designer.cs";
            var          fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Orchestration", generateFilePathInfo.SolutionInfo.NamespaceNameForOrchestration);

            var targetFilePath = generateFilePathInfo.SolutionInfo.OrchestrationProjectFolder + "Extensions.designer.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);

            targetFilePath = generateFilePathInfo.SolutionInfo.OrchestrationProjectFolder + "Extensions.cs";
            if (File.Exists(targetFilePath) == false)
            {
                Util.WriteFileIfContentNotEqual(targetFilePath,
                                                @"
namespace " + generateFilePathInfo.SolutionInfo.NamespaceNameForOrchestration + @"
{
    static partial class Extensions
    {
    }
}
");
            }
        }
        #endregion
    }
}