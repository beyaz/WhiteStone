using System.IO;
using BOA.CodeGeneration.Util;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOAPlugins.VSIntegration
{
    public static class FormAssistantProjectInitializer
    {
        
        #region Properties
        static TFSAccessForBOA Tfs => new TFSAccessForBOA();
        #endregion

        #region Public Methods
        public static void Initialize(SolutionInfo solutionInfo)
        {
            Export_FormAssistant_cs(solutionInfo);
            Export_FormAssistant_tsx(solutionInfo);
            Export_Orchestration_Extension_Class(solutionInfo);
        }
        #endregion

        #region Methods
        static void Export_FormAssistant_cs(SolutionInfo solutionInfo)
        {
            

            // FormAssistant.cs 
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard/BOA.Types.CardGeneral.DebitCard/FormAssistant.cs";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardGeneral.DebitCard", NamingInfo.GetNamespaceNameForType(solutionInfo.SlnFilePath));

            
            var targetFilePath = solutionInfo.FilePathOf_FormAssistant_cs_In_Types;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_FormAssistant_tsx(SolutionInfo solutionInfo)
        {
            // FormAssistant.tsx
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard/One/BOA.One.CardGeneral.DebitCard/ClientApp/utils/FormAssistant.tsx";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardGeneral.DebitCard", NamingInfo.GetNamespaceNameForType(solutionInfo.SlnFilePath));

            var targetFilePath = solutionInfo.FormAssistant_tsx_FilePath;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_Orchestration_Extension_Class(SolutionInfo solutionInfo)
        {
            // Extensions.designer.cs
            const string tfsPath     = @"$/BOA.BusinessModules/Dev/BOA.CardGeneral.DebitCard/BOA.Orchestration.CardGeneral.DebitCard/Extensions.designer.cs";
            var          fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Orchestration", NamingInfo.GetNamespaceNameForOrchestration(solutionInfo.SlnFilePath));

            var targetFilePath = solutionInfo.OrchestrationProjectFolder + "Extensions.designer.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);

            targetFilePath = solutionInfo.OrchestrationProjectFolder + "Extensions.cs";
            if (File.Exists(targetFilePath) == false)
            {
                Util.WriteFileIfContentNotEqual(targetFilePath,
                                                @"
namespace " + NamingInfo.GetNamespaceNameForOrchestration(solutionInfo.SlnFilePath) + @"
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