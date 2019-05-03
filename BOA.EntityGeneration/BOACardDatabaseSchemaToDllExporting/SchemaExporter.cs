using System.IO;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class BatExporter
    {
        [Inject]
        public NamingHelper NamingHelper { get; set; }

        [Inject]
        public ProjectExportLocation ProjectExportLocation { get; set; }

        #region Public Methods
        public void Export(string schemaName)
        {
            const string dir = @"\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\BOACardEntityGeneration\";
            const string targetLocalDir = @"d:\boa\BOACard.EntityGeneration\";

            var projectLocationParentDir = ProjectExportLocation.GetExportLocationOfTypeProject(schemaName);

            var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(schemaName);
            var typeClassNamespace = NamingHelper.GetTypeClassNamespace(schemaName);

            


            var content = $@"
cd\
cd windows
cd system32

robocopy ""{dir}Generator"" ""{targetLocalDir}"" /E

{targetLocalDir}BOACardEntityGenerationWrapper.exe %~n0

""%programfiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"" ""{projectLocationParentDir}{typeClassNamespace}\{typeClassNamespace}.csproj""
""%programfiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"" ""{projectLocationParentDir}{businessClassNamespace}\{businessClassNamespace}.csproj""

if NOT [""%errorlevel%""]==[""0""] pause

";
            File.WriteAllText(dir + schemaName + ".bat", content);
        }
        #endregion
    }

    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public AllBusinessClassesInOne AllBusinessClassesInOne { get; set; }

        [Inject]
        public AllTypeClassesInOne AllTypeClassesInOne { get; set; }

        [Inject]
        public BatExporter BatExporter { get; set; }

        [Inject]
        public BusinessDllCompiler BusinessDllCompiler { get; set; }

        [Inject]
        public BusinessProjectExporter BusinessProjectExporter { get; set; }

        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        [Inject]
        public GeneratorOfTypeClass GeneratorOfTypeClass { get; set; }

        [Inject]
        public TypeDllCompiler TypeDllCompiler { get; set; }

        [Inject]
        public TypesProjectExporter TypesProjectExporter { get; set; }
        #endregion

        #region Public Methods
        public void Export(string schemaName)
        {
            ExportTypeDll(schemaName);
            ExportBusinessDll(schemaName);

            BatExporter.Export(schemaName);
        }
        #endregion

        #region Methods
        void ExportBusinessDll(string schemaName)
        {
            var code = AllBusinessClassesInOne.GetCode(schemaName);

            BusinessProjectExporter.Export(schemaName, code);

            // BusinessDllCompiler.Compile(schemaName, code);
        }

        void ExportTypeDll(string schemaName)
        {
            var code = AllTypeClassesInOne.GetCode(schemaName);

            TypesProjectExporter.Export(schemaName, code);

            //TypeDllCompiler.Compile(schemaName, code);
        }
        #endregion
    }
}