using System.IO;
using BOA.CodeGeneration.Util;

namespace BOAPlugins.FormApplicationGenerator
{
    static class FormAssistantProjectInitializer
    {
        #region Properties
        static TFSAccessForBOA Tfs => new TFSAccessForBOA();
        #endregion

        #region Public Methods
        public static void Initialize(Model model)
        {
            // Extensions.designer.cs
            var tfsPath     = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Orchestration.CardPaymentSystem.Clearing/Extensions.designer.cs";
            var fileContent = Tfs.GetFileContent(tfsPath).Replace("BOA.Orchestration.CardPaymentSystem.Clearing", model.NamespaceNameForOrchestration);

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

            // FormAssistant.cs 
            tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/BOA.Types.CardPaymentSystem.Clearing/FormAssistant.cs";

            fileContent = Tfs.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", model.NamespaceNameForType);

            targetFilePath = model.TypesProjectFolder + "FormAssistant.cs";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);

            // FormAssistant.tsx
            tfsPath = @"$/BOA.BusinessModules/Dev/BOA.CardPaymentSystem.Clearing/One/BOA.One.Office.CardPaymentSystem.Clearing/ClientApp/utils/FormAssistant.tsx";

            fileContent = Tfs.GetFileContent(tfsPath).Replace("BOA.Types.CardPaymentSystem.Clearing", model.NamespaceNameForType);

            targetFilePath = model.OneProjectFolder + @"ClientApp\utils\FormAssistant.tsx";

            Util.WriteFileIfContentNotEqual(targetFilePath, fileContent);
        }
        #endregion
    }
}