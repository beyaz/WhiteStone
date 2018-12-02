using System.IO;
using BOA.CodeGeneration.Util;
using BOAPlugins.FormApplicationGenerator.UI;
using BOAPlugins.Utility;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    public static class FormAssistantProjectInitializer
    {
        #region Properties
        static TFSAccessForBOA Tfs => new TFSAccessForBOA();
        #endregion

        #region Public Methods
        public static void Initialize(MainWindowModel mainWindowModel)
        {
            Export_FormAssistant_cs(mainWindowModel);
            Export_FormAssistant_tsx(mainWindowModel);
            Export_Orchestration_Extension_Class(mainWindowModel);
        }
        #endregion

        #region Methods
        static void Export_FormAssistant_cs(MainWindowModel mainWindowModel)
        {
            // FormAssistant.cs 
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Types.CardPaymentSystem.Clearing/FormAssistant.cs";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", mainWindowModel.NamingInfo.NamespaceNameForType);

            var targetFilePath = mainWindowModel.SolutionInfo.FilePathOf_FormAssistant_cs_In_Types;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_FormAssistant_tsx(MainWindowModel mainWindowModel)
        {
            // FormAssistant.tsx
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/One/BOA.One.Office.CardPaymentSystem.Clearing/ClientApp/utils/FormAssistant.tsx";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", mainWindowModel.NamingInfo.NamespaceNameForType);

            var targetFilePath = mainWindowModel.SolutionInfo.FormAssistant_tsx_FilePath;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_Orchestration_Extension_Class(MainWindowModel mainWindowModel)
        {
            // Extensions.designer.cs
            const string tfsPath     = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Orchestration.CardPaymentSystem.Clearing/Extensions.designer.cs";
            var          fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Orchestration.CardPaymentSystem.Clearing", mainWindowModel.NamingInfo.NamespaceNameForOrchestration);

            var targetFilePath = mainWindowModel.SolutionInfo.OrchestrationProjectFolder + "Extensions.designer.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);

            targetFilePath = mainWindowModel.SolutionInfo.OrchestrationProjectFolder + "Extensions.cs";
            if (File.Exists(targetFilePath) == false)
            {
                Util.WriteFileIfContentNotEqual(targetFilePath,
                                                @"
namespace " + mainWindowModel.NamingInfo.NamespaceNameForOrchestration + @"
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