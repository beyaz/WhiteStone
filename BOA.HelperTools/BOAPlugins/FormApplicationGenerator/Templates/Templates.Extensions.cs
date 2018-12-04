using System.Collections.Generic;
using System.Linq;
using BOAPlugins.FormApplicationGenerator.Types;

namespace BOAPlugins.FormApplicationGenerator.Templates
{
    partial class BParameterComponentTemplate
    {
        public BParameterComponent Data { get; set; }
    }
    
    partial class BInputNumericTemplate
    {
        public BInputNumeric Data { get; set; }
    }
    partial class BDateTimePickerTemplate
    {
        public BDateTimePicker Data { get; set; }
    }
    partial class BAccountComponentTemplate
    {
        public BAccountComponent Data { get; set; }
    }
    
    partial class BInputTemplate
    {
        public BInput Data { get; set; }
    }

    partial class OrchestrationFileForDetailForm
    {
        public string ClassName { get; set; }
    }
    partial class TabPageTemplate
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