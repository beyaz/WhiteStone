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
        public static void Initialize(Model model)
        {
            Export_FormAssistant_cs(model);
            Export_FormAssistant_tsx(model);
            Export_Orchestration_Extension_Class(model);
        }
        #endregion

        #region Methods
        static void Export_FormAssistant_cs(Model model)
        {
            // FormAssistant.cs 
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Types.CardPaymentSystem.Clearing/FormAssistant.cs";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", model.NamingInfo.NamespaceNameForType);

            var targetFilePath = model.SolutionInfo.FilePathOf_FormAssistant_cs_In_Types;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_FormAssistant_tsx(Model model)
        {
            // FormAssistant.tsx
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/One/BOA.One.Office.CardPaymentSystem.Clearing/ClientApp/utils/FormAssistant.tsx";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", model.NamingInfo.NamespaceNameForType);

            var targetFilePath = model.SolutionInfo.FormAssistant_tsx_FilePath;

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_Orchestration_Extension_Class(Model model)
        {
            // Extensions.designer.cs
            const string tfsPath     = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Orchestration.CardPaymentSystem.Clearing/Extensions.designer.cs";
            var          fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Orchestration.CardPaymentSystem.Clearing", model.NamingInfo.NamespaceNameForOrchestration);

            var targetFilePath = model.SolutionInfo.OrchestrationProjectFolder + "Extensions.designer.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);

            targetFilePath = model.SolutionInfo.OrchestrationProjectFolder + "Extensions.cs";
            if (File.Exists(targetFilePath) == false)
            {
                Util.WriteFileIfContentNotEqual(targetFilePath,
                                                @"
namespace " + model.NamingInfo.NamespaceNameForOrchestration + @"
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