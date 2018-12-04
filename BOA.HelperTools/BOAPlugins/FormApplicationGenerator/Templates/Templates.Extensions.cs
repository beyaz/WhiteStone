using System.Collections.Generic;
using BOAPlugins.FormApplicationGenerator.Types;

namespace BOAPlugins.FormApplicationGenerator.Templates
{ partial class  BCheckBoxTemplate
    {
        public BCheckBox Data { get; set; }
    }
    partial class  BBranchComponentTemplate
    {
        public BBranchComponent Data { get; set; }
    }
    partial class BParameterComponentTemplate
    {
        #region Public Properties
        public BParameterComponent Data { get; set; }
        #endregion
    }

    partial class BInputNumericTemplate
    {
        #region Public Properties
        public BInputNumeric Data { get; set; }
        #endregion
    }

    partial class BDateTimePickerTemplate
    {
        #region Public Properties
        public BDateTimePicker Data { get; set; }
        #endregion
    }

    partial class BAccountComponentTemplate
    {
        #region Public Properties
        public BAccountComponent Data { get; set; }
        #endregion
    }

    partial class BInputTemplate
    {
        #region Public Properties
        public BInput Data { get; set; }
        #endregion
    }

    partial class BCardTemplate
    {
        #region Public Properties
        public int?                  ColumnIndex { get; set; }
        public IReadOnlyList<BField> Components  { get; set; }
        public string                Title       { get; set; }
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

    partial class BInputMaskTemplate
    {
        public BInputMask Data { get; set; }
    }
}