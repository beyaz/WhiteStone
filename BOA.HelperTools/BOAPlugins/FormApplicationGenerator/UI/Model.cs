using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    [Serializable]
    public class Model
    {
        #region Constructors
        public Model(string solutionFilePath, string formName)
        {
            SolutionFilePath = solutionFilePath ?? throw new ArgumentNullException(nameof(solutionFilePath));
            FormName         = formName ?? throw new ArgumentNullException(nameof(formName));

            var solutionFileName = Path.GetFileName(solutionFilePath);
            var namespaceName    = solutionFileName.RemoveFromStart("BOA.").RemoveFromEnd(".sln");

            NamespaceName                 = namespaceName;
            RequestNameForDefinition      = formName + "FormRequest";
            RequestNameForList            = formName + "ListFormRequest";
            NamespaceNameForType          = "BOA.Types." + namespaceName;
            NamespaceNameForOrchestration = "BOA.Orchestration." + namespaceName;
            DefinitionFormDataClassName   = formName + "FormData";
            TypesProjectFolder            = Path.GetDirectoryName(solutionFilePath) + Path.DirectorySeparatorChar + NamespaceNameForType + Path.DirectorySeparatorChar;

            OrchestrationProjectFolder = Path.GetDirectoryName(SolutionFilePath) + Path.DirectorySeparatorChar + NamespaceNameForOrchestration + Path.DirectorySeparatorChar;

            OneProjectFolder = GetOneProjectFolder(SolutionFilePath, NamespaceName);
        }
        #endregion

        #region Public Properties
        public IReadOnlyCollection<BCard> Cards                       { get; set; } = new List<BCard>();
        public string                     DefinitionFormDataClassName { get; }

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

        public bool UserRawStringForMessaging { get; set; }
        public string                      FormName                      { get; }
        public bool                        IsTabForm                     { get; set; }
        public IReadOnlyCollection<BField> ListFormSearchFields          { get; set; } = new List<BField>();
        public string                      NamespaceName                 { get; }
        public string                      NamespaceNameForOrchestration { get; }
        public string                      NamespaceNameForType          { get; }
        public string                      OneProjectFolder              { get; }
        public string                      OrchestrationProjectFolder    { get; }
        public string                      RequestNameForDefinition      { get; }
        public string                      RequestNameForList            { get; }
        public string                      SolutionFilePath              { get; }
        public IReadOnlyCollection<BTab>   Tabs                          { get; set; } = new List<BTab>();
        public string                      TypesProjectFolder            { get; }
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