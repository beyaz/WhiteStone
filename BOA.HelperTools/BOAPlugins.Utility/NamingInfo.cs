using System;
using System.IO;
using BOA.Common.Helpers;

namespace BOAPlugins.Utility
{
    [Serializable]
    public class NamingInfo
    {
        #region Public Properties
        public string DefinitionFormDataClassName        { get; private set; }
        public string NamespaceNameForOrchestration      { get; private set; }
        public string NamespaceNameForType               { get; private set; }
        public string OrchestrationFileNameForDetailForm { get; private set; }

        public string OrchestrationFileNameForListForm { get; private set; }
        public string RequestNameForDefinition         { get; private set; }
        public string RequestNameForList               { get; private set; }
        public string TableNameInDatabase              { get; private set; }
        public string TsxFileNameForDetailForm         { get; private set; }
        public string TsxFileNameForListForm           { get; private set; }
        public string TypeAssemblyName                 { get; private set; }
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
                NamespaceNameForType               = GetNamespaceNameForType(slnFilePath),
                TypeAssemblyName                   = $"{GetNamespaceNameForType(slnFilePath)}.dll",
                NamespaceNameForOrchestration      = GetNamespaceNameForOrchestration(slnFilePath),
                DefinitionFormDataClassName        = tableNameIndDatabase + "FormData",
                OrchestrationFileNameForListForm   = tableNameIndDatabase + "ListForm.cs",
                OrchestrationFileNameForDetailForm = tableNameIndDatabase + "Form.cs",

                TsxFileNameForListForm   = tableNameIndDatabase + "ListForm.tsx",
                TsxFileNameForDetailForm = tableNameIndDatabase + "Form.tsx"
            };

            return info;
        }

        public static string GetNamespaceNameForOrchestration(string slnFilePath)
        {
            return $"BOA.Orchestration.{GetSolutionNamespaceName(slnFilePath)}";
        }

        public static string GetNamespaceNameForType(string slnFilePath)
        {
            return $"BOA.Types.{GetSolutionNamespaceName(slnFilePath)}";
        }

        public static string GetSolutionNamespaceName(string slnFilePath)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            return Path.GetFileName(slnFilePath).RemoveFromStart("BOA.").RemoveFromEnd(".sln");
        }
        #endregion
    }
}