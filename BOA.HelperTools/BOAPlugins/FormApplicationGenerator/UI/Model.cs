using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.TypescriptModelGeneration;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    [Serializable]
    public class NamingInfo
    {
        public static string GetSolutionNamespaceName(string solutionFilePath)
        {
            if (solutionFilePath == null)
            {
                throw new ArgumentNullException(nameof(solutionFilePath));
            }

            return Path.GetFileName(solutionFilePath).RemoveFromStart("BOA.").RemoveFromEnd(".sln");
        }
        #region Public Properties
        public string DefinitionFormDataClassName   { get; private set; }
        public string NamespaceName                 { get; private set; }
        public string NamespaceNameForOrchestration { get; private set; }
        public string NamespaceNameForType          { get; private set; }
        public string RequestNameForDefinition      { get; private set; }
        public string RequestNameForList            { get; private set; }
        public string TableNameInDatabase           { get; private set; }
        public string TypeAssemblyName              { get; private set; }
        #endregion

        #region Public Methods
        public static NamingInfo Create(string solutionFilePath, string tableNameIndDatabase)
        {
            if (solutionFilePath == null)
            {
                throw new ArgumentNullException(nameof(solutionFilePath));
            }

            if (tableNameIndDatabase == null)
            {
                throw new ArgumentNullException(nameof(tableNameIndDatabase));
            }

            var namespaceName = GetSolutionNamespaceName(solutionFilePath);

            var info = new NamingInfo
            {
                TableNameInDatabase           = tableNameIndDatabase,
                NamespaceName                 = namespaceName,
                RequestNameForDefinition      = tableNameIndDatabase + "FormRequest",
                RequestNameForList            = tableNameIndDatabase + "ListFormRequest",
                NamespaceNameForType          = $"BOA.Types.{namespaceName}",
                TypeAssemblyName              = $"BOA.Types.{namespaceName}.dll",
                NamespaceNameForOrchestration = "BOA.Orchestration." + namespaceName,
                DefinitionFormDataClassName   = tableNameIndDatabase + "FormData"
            };

            return info;
        }
        #endregion
    }

    [Serializable]
    public class Model
    {
        public NamingInfo NamingInfo { get; set; }
        public SolutionInfo SolutionInfo { get; set; }


        #region Constructors
        public Model(string solutionFilePath, string tableNameIndDatabase)
        {

            NamingInfo = NamingInfo.Create(solutionFilePath,tableNameIndDatabase);
            SolutionInfo = SolutionInfo.CreateFrom(solutionFilePath);

            SolutionFilePath = solutionFilePath;

            TableNameInDatabase = tableNameIndDatabase;


            TypesProjectFolder            = Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + NamingInfo.NamespaceNameForType + Path.DirectorySeparatorChar;

            OrchestrationProjectFolder = Path.GetDirectoryName(SolutionFilePath) + Path.DirectorySeparatorChar + NamingInfo.NamespaceNameForOrchestration + Path.DirectorySeparatorChar;

            
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
        public string                      OrchestrationProjectFolder    { get; }
        public string                      SolutionFilePath              { get; }

        public string                    TableNameInDatabase { get; }
        public IReadOnlyCollection<BTab> Tabs                { get; set; } = new List<BTab>();
        public string                    TypesProjectFolder  { get; }

        public bool UserRawStringForMessaging { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}