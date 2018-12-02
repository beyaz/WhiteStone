using System.IO;
using BOA.CodeGeneration.Util;
using BOAPlugins.FormApplicationGenerator.UI;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    static class FormAssistantProjectInitializer
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

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", model.NamespaceNameForType);

            var targetFilePath = model.TypesProjectFolder + "FormAssistant.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_FormAssistant_tsx(Model model)
        {
            // FormAssistant.tsx
            const string tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/One/BOA.One.Office.CardPaymentSystem.Clearing/ClientApp/utils/FormAssistant.tsx";

            var fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", model.NamespaceNameForType);

            var targetFilePath = model.OneProjectFolder + @"ClientApp\utils\FormAssistant.tsx";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }

        static void Export_Orchestration_Extension_Class(Model model)
        {
            // Extensions.designer.cs
            const string tfsPath     = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Orchestration.CardPaymentSystem.Clearing/Extensions.designer.cs";
            var          fileContent = TFSAccessForBOA.GetFileContent(tfsPath).Replace("BOA.Orchestration.CardPaymentSystem.Clearing", model.NamespaceNameForOrchestration);

            var targetFilePath = model.OrchestrationProjectFolder + "Extensions.designer.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);

            targetFilePath = model.OrchestrationProjectFolder + "Extensions.cs";
            if (File.Exists(targetFilePath) == false)
            {
                Util.WriteFileIfContentNotEqual(targetFilePath,
                                                @"
namespace " + model.NamespaceNameForOrchestration + @"
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