using System.IO;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
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
            const string dir            = @"\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\BOACardEntityGeneration\";
            const string targetLocalDir = @"d:\boa\BOACard.EntityGeneration\";

            var projectLocationParentDir = ProjectExportLocation.GetExportLocationOfTypeProject(schemaName);

            var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(schemaName);
            var typeClassNamespace     = NamingHelper.GetTypeClassNamespace(schemaName);

            


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
}