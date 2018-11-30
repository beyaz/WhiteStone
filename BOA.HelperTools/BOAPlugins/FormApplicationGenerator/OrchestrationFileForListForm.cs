using System.Collections.Generic;
using System.Linq;

namespace BOAPlugins.FormApplicationGenerator
{
    partial class OrchestrationFile
    {
        #region Public Properties
        public string                ClassName                   { get; set; }
        public string                DefinitionFormDataClassName { get; set; }
        public IReadOnlyList<string> GridColumnFields            { get; set; }
        public string                NamespaceName               { get; set; }
        public string                NamespaceNameForType        { get; set; }
        public string                RequestName                 { get; set; }
        #endregion
    }

    static class OrchestrationFileForListForm
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            var orchestrationFileTemplate = new OrchestrationFile
            {
                NamespaceNameForType        = Model.NamespaceNameForType,
                NamespaceName               = Model.NamespaceNameForOrchestration,
                ClassName                   = Model.FormName + "ListForm",
                RequestName                 = Model.RequestNameForList,
                DefinitionFormDataClassName = Model.DefinitionFormDataClassName,
                GridColumnFields            = Model.FormDataClassFields.Select(fieldInfo => fieldInfo.Name).ToArray()
            };
            return orchestrationFileTemplate.TransformText();
        }
        #endregion
    }
}