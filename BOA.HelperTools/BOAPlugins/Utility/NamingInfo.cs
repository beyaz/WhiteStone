using System;

namespace BOAPlugins.Utility
{
    [Serializable]
    public class NamingInfo
    {
        #region Public Properties
        public string DefinitionFormDataClassName        { get; private set; }
        public string OrchestrationFileNameForDetailForm { get; private set; }
        public string OrchestrationFileNameForListForm   { get; private set; }
        public string RequestNameForDefinition           { get; private set; }
        public string RequestNameForList                 { get; private set; }
        public string TableNameInDatabase                { get; private set; }
        public string TsxFileNameForDetailForm           { get; private set; }
        public string TsxFileNameForListForm             { get; private set; }
        #endregion

        #region Public Methods
        public static NamingInfo Create(string slnFilePath, string tableNameIndDatabase)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            if (tableNameIndDatabase == null)
            {
                throw new ArgumentNullException(nameof(tableNameIndDatabase));
            }

            var info = new NamingInfo
            {
                TableNameInDatabase                = tableNameIndDatabase,
                RequestNameForDefinition           = tableNameIndDatabase + "FormRequest",
                RequestNameForList                 = tableNameIndDatabase + "ListFormRequest",
                DefinitionFormDataClassName        = tableNameIndDatabase + "FormData",
                OrchestrationFileNameForListForm   = tableNameIndDatabase + "ListForm.cs",
                OrchestrationFileNameForDetailForm = tableNameIndDatabase + "Form.cs",
                TsxFileNameForListForm             = tableNameIndDatabase + "ListForm.tsx",
                TsxFileNameForDetailForm           = tableNameIndDatabase + "Form.tsx"
            };

            return info;
        }
        #endregion
    }
}