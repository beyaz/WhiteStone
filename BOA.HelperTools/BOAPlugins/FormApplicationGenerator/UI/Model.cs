using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    [Serializable]
    public class NamingInfo
    {
        public static string GetNamespaceNameForType(string slnFilePath)
        {
            return $"BOA.Types.{GetSolutionNamespaceName(slnFilePath)}";
        }
        public static string GetNamespaceNameForOrchestration(string slnFilePath)
        {
            return $"BOA.Orchestration.{GetSolutionNamespaceName(slnFilePath)}";
        }
        
        public static string GetSolutionNamespaceName(string slnFilePath)
        {
            if (slnFilePath == null)
            {
                throw new ArgumentNullException(nameof(slnFilePath));
            }

            return Path.GetFileName(slnFilePath).RemoveFromStart("BOA.").RemoveFromEnd(".sln");
        }
        #region Public Properties
        public string DefinitionFormDataClassName   { get; private set; }
        public string NamespaceNameForOrchestration { get; private set; }
        public string NamespaceNameForType          { get; private set; }
        public string RequestNameForDefinition      { get; private set; }
        public string RequestNameForList            { get; private set; }
        public string TableNameInDatabase           { get; private set; }
        public string TypeAssemblyName              { get; private set; }

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
                TableNameInDatabase           = tableNameIndDatabase,
                RequestNameForDefinition      = tableNameIndDatabase + "FormRequest",
                RequestNameForList            = tableNameIndDatabase + "ListFormRequest",
                NamespaceNameForType          = GetNamespaceNameForType(slnFilePath),
                TypeAssemblyName              = $"{GetNamespaceNameForType(slnFilePath)}.dll",
                NamespaceNameForOrchestration = GetNamespaceNameForOrchestration(slnFilePath),
                DefinitionFormDataClassName   = tableNameIndDatabase + "FormData",
                OrchestrationFileNameForListForm = tableNameIndDatabase + "ListForm.cs",
                OrchestrationFileNameForDetailForm = tableNameIndDatabase + "Form.cs",

                TsxFileNameForListForm   = tableNameIndDatabase + "ListForm.tsx",
                TsxFileNameForDetailForm = tableNameIndDatabase + "Form.tsx"
                
            };

            return info;
        }
        public string TsxFileNameForListForm { get; private set; }
        public string TsxFileNameForDetailForm { get; private set; }

        public string OrchestrationFileNameForListForm { get; private set; }
        public string OrchestrationFileNameForDetailForm { get; private set; }
        #endregion
    }

    [Serializable]
    public class Model
    {



        public string ListFormTsxFilePath => SolutionInfo.OneProjectFolder + @"ClientApp\pages\" + NamingInfo.TsxFileNameForListForm;
        public string DetailFormTsxFilePath => SolutionInfo.OneProjectFolder + @"ClientApp\pages\" + NamingInfo.TsxFileNameForDetailForm;


        public NamingInfo NamingInfo { get; set; }
        public SolutionInfo SolutionInfo { get; set; }


        #region Constructors
        public Model(string solutionFilePath, string tableNameIndDatabase)
        {

            NamingInfo = NamingInfo.Create(solutionFilePath,tableNameIndDatabase);
            SolutionInfo = SolutionInfo.CreateFrom(solutionFilePath);


            TableNameInDatabase = tableNameIndDatabase;


            

            

            
        }
        #endregion

        #region Public Properties
        public IReadOnlyCollection<BCard> Cards                       { get; set; } = new List<BCard>();

        public IReadOnlyCollection<BField> FormDataClassFields
        {
            get
            {
                if (IsTabForm)
                {
                    return Tabs.GetAllFields();
                }

                return Cards.GetAllFields();
            }
        }

        public bool                        IsTabForm                     { get; set; }
        public IReadOnlyCollection<BField> ListFormSearchFields          { get; set; } = new List<BField>();

        public string                    TableNameInDatabase { get; }
        public IReadOnlyCollection<BTab> Tabs                { get; set; } = new List<BTab>();

        public bool UserRawStringForMessaging { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}