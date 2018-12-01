using System.Collections.Generic;

namespace BOAPlugins.FormApplicationGenerator
{
    partial class JSXElementForRenderBrowsePage
    {
        #region Public Properties
        public IReadOnlyList<BoaJsxComponentRenderTemplate> Components { get; set; }
        #endregion
    }

    partial class BCardTemplate
    {
        #region Public Properties
        public int?                                       ColumnIndex { get; set; }
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
        public IReadOnlyList<BCardTemplate> Cards { get; set; }
    }
}