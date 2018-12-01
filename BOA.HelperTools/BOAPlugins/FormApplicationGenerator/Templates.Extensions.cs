using System.Collections.Generic;
using System.Linq;
using BOAPlugins.FormApplicationGenerator.Types;

namespace BOAPlugins.FormApplicationGenerator
{partial class TabPageTemplate
    {
        public BCardSectionTemplate content { get; set; }
        public string text { get; set; }
    }
    partial class BTabControlTemplate
    {
        public IReadOnlyList<TabPageTemplate> TabPages{ get; set; }
    }
    partial class TransactionPageTemplate
    {
        public bool HasWorkFlow { get; set; }
        public bool IsTabForm { get; set; }
        public string                        NamespaceNameForType { get; set; }
        public string                        RequestName          { get; set; }
        public string                        ClassName            { get; set; }
        public string                        DetailFormClassName  { get; set; }
        public IReadOnlyList<SnapInfo>       Snaps                { get; set; }
     
        public BCardSectionTemplate ContentAsBCardSection { get; set; }
        public BTabControlTemplate ContentAsTabControl { get; set; }
    }
    partial class BrowsePageTemplate
    {
        public string NamespaceNameForType { get; set; }
        public string RequestName { get; set; }
        public string ClassName { get; set; }
        public string DetailFormClassName { get; set; }
        public IReadOnlyList<SnapInfo> Snaps { get; set; }
        public IReadOnlyList<BFieldTemplate> Components { get; set; }
    }
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

    partial class BFieldTemplate
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

    

    partial class BCardTemplate
    {
        #region Public Properties
        public int?                                         ColumnIndex { get; set; }
        public IReadOnlyList<BFieldTemplate> Components  { get; set; }
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