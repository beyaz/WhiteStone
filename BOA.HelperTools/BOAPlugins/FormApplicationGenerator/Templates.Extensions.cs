using System.Collections.Generic;
using System.Linq;

namespace BOAPlugins.FormApplicationGenerator
{
    partial class OrchestrationFileForListForm
    {
        #region Public Properties
        public string                ClassName                   { get; set; }
        public string                DefinitionFormDataClassName { get; set; }
        public IReadOnlyList<string> GridColumnFields            { get; set; }
        public string                NamespaceName               { get; set; }
        public string                NamespaceNameForType        { get; set; }
        public string                RequestName                 { get; set; }
        #endregion

        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            var orchestrationFileTemplate = new OrchestrationFileForListForm
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

    partial class BoaJsxComponentRenderTemplate
    {
        #region Public Properties
        public bool   IsBAccountComponent    { get; set; }
        public bool   IsBBranchComponent     { get; set; }
        public bool   IsBCheckBox            { get; set; }
        public bool   IsBDateTimePicker      { get; set; }
        public bool   IsBInput               { get; set; }
        public bool   IsBInputNumeric        { get; set; }
        public bool   IsBInputNumericDecimal { get; set; }
        public bool   IsBParameterComponent  { get; set; }
        public string Label                  { get; set; }
        public string ParamType              { get; set; }
        public string SnapName               { get; set; }
        public string ValueAccessPath        { get; set; }
        public bool   ValueTypeIsInt32       { get; set; }
        #endregion
    }

    partial class JSXElementForRenderBrowsePage
    {
        #region Public Properties
        public IReadOnlyList<BoaJsxComponentRenderTemplate> Components { get; set; }
        #endregion
    }

    partial class BCardTemplate
    {
        #region Public Properties
        public int?                                         ColumnIndex { get; set; }
        public IReadOnlyList<BoaJsxComponentRenderTemplate> Components  { get; set; }
        public string                                       Title       { get; set; }
        #endregion

        #region Properties
        bool HasColumnIndex => ColumnIndex.HasValue;
        bool HasTitle       => Title != null;
        #endregion
    }

    partial class BCardSectionTemplate
    {
        #region Public Properties
        public IReadOnlyList<BCardTemplate> Cards { get; set; }
        #endregion
    }
}