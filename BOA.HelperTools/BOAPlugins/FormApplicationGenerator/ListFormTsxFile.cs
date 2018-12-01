using System.Linq;

namespace BOAPlugins.FormApplicationGenerator
{
    static class ListFormTsxFile
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            var template = new BrowsePageTemplate
            {
                NamespaceNameForType = Model.NamespaceNameForType,
                RequestName          = Model.RequestNameForList,
                ClassName            = Model.FormName + @"ListForm",
                DetailFormClassName  = Model.FormName + @"Form",
                Snaps                = Model.ListFormSearchFields.GetSnaps(),
                Components = Model.ListFormSearchFields.Select(Map.GetRenderComponent).ToList()
            };

            return template.TransformText();
        }
        #endregion
    }
}