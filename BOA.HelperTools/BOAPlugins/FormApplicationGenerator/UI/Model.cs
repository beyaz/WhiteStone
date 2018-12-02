using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    [Serializable]
    public class NamingInfo
    {
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

            var namespaceName = Path.GetFileName(solutionFilePath).RemoveFromStart("BOA.").RemoveFromEnd(".sln");

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

        #region Constructors
        public Model(string solutionFilePath, string tableNameIndDatabase)
        {

            NamingInfo = NamingInfo.Create(solutionFilePath,tableNameIndDatabase);


            SolutionFilePath    = solutionFilePath ?? throw new ArgumentNullException(nameof(solutionFilePath));
            TableNameInDatabase = tableNameIndDatabase ?? throw new ArgumentNullException(nameof(tableNameIndDatabase));

            var solutionFileName = Path.GetFileName(solutionFilePath);
            var namespaceName    = solutionFileName.RemoveFromStart("BOA.").RemoveFromEnd(".sln");

            NamespaceName                 = namespaceName;
            RequestNameForList            = tableNameIndDatabase + "ListFormRequest";
            NamespaceNameForType          = $"BOA.Types.{namespaceName}";
            TypeAssemblyName              = $"BOA.Types.{namespaceName}.dll";
            NamespaceNameForOrchestration = "BOA.Orchestration." + namespaceName;
            TypesProjectFolder            = Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + NamespaceNameForType + Path.DirectorySeparatorChar;

            OrchestrationProjectFolder = Path.GetDirectoryName(SolutionFilePath) + Path.DirectorySeparatorChar + NamespaceNameForOrchestration + Path.DirectorySeparatorChar;

            OneProjectFolder = GetOneProjectFolder(SolutionFilePath, NamespaceName);
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
        public string                      NamespaceName                 { get; }
        public string                      NamespaceNameForOrchestration { get; }
        public string                      NamespaceNameForType          { get; }
        public string                      OneProjectFolder              { get; }
        public string                      OrchestrationProjectFolder    { get; }
        public string                      RequestNameForList            { get; }
        public string                      SolutionFilePath              { get; }

        public string                    TableNameInDatabase { get; }
        public IReadOnlyCollection<BTab> Tabs                { get; set; } = new List<BTab>();
        public string                    TypeAssemblyName    { get; set; }
        public string                    TypesProjectFolder  { get; }

        public bool UserRawStringForMessaging { get; set; }
        #endregion

        #region Methods
        static string GetOneProjectFolder(string solutionFilePath, string namespaceName)
        {
            var paths = new[]
            {
                Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One.Office." + namespaceName + Path.DirectorySeparatorChar,
                Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + @"One\BOA.One." + namespaceName + Path.DirectorySeparatorChar
            };

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    return path;
                }
            }

            throw new InvalidOperationException("One project folder not found." + string.Join(Environment.NewLine, paths));
        }
        #endregion
    }
}