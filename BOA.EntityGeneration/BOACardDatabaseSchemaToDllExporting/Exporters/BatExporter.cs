using System;
using System.Collections.Generic;
using System.IO;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class BatFileLocationInfo
    {
        #region Public Properties
        public string MsBuildExe     { get; set; } = @"%programfiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe";
        public string SharedDir      { get; set; } = @"\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\BOACardEntityGeneration\";
        public string TargetLocalDir { get; set; } = @"d:\boa\BOACard.EntityGeneration\";
        #endregion
    }

    public class BatExporter
    {
        #region Public Properties
        [Inject]
        public BatFileLocationInfo BatFileLocationInfo { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }

        [Inject]
        public ProjectExportLocation ProjectExportLocation { get; set; }
        #endregion

        #region Public Methods
        public void Export(string schemaName)
        {
            var projectLocationParentDir = ProjectExportLocation.GetExportLocationOfTypeProject(schemaName);

            var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(schemaName);

            var typeClassNamespace = NamingHelper.GetTypeClassNamespace(schemaName);

            var content = $@"
cd\
cd windows
cd system32

robocopy ""{BatFileLocationInfo.SharedDir}Generator"" ""{BatFileLocationInfo.TargetLocalDir}"" /E

{BatFileLocationInfo.TargetLocalDir}BOACardEntityGenerationWrapper.exe %~n0

""{BatFileLocationInfo.MsBuildExe}"" ""{projectLocationParentDir}{typeClassNamespace}\{typeClassNamespace}.csproj""
""{BatFileLocationInfo.MsBuildExe}"" ""{projectLocationParentDir}{businessClassNamespace}\{businessClassNamespace}.csproj""

if NOT [""%errorlevel%""]==[""0""] pause

";
            File.WriteAllText(BatFileLocationInfo.SharedDir + schemaName + ".bat", content);
        }

        [Inject]
        public BOACardDatabaseSchemaNames SchemaNames { get; set; }

        public void ExportAllInOne()
        {
            var schemaNames = SchemaNames.SchemaNames;

            var lines = new List<string>
            {
                @"cd\
cd windows
cd system32
robocopy ""{BatFileLocationInfo.SharedDir}Generator"" ""{BatFileLocationInfo.TargetLocalDir}"" /E
"
            };

            foreach (var schemaName in schemaNames)
            {
                var projectLocationParentDir = ProjectExportLocation.GetExportLocationOfTypeProject(schemaName);

                var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(schemaName);

                var typeClassNamespace = NamingHelper.GetTypeClassNamespace(schemaName);

                lines.Add($@"


{BatFileLocationInfo.TargetLocalDir}BOACardEntityGenerationWrapper.exe {schemaName}

""{BatFileLocationInfo.MsBuildExe}"" ""{projectLocationParentDir}{typeClassNamespace}\{typeClassNamespace}.csproj""
""{BatFileLocationInfo.MsBuildExe}"" ""{projectLocationParentDir}{businessClassNamespace}\{businessClassNamespace}.csproj""

");
            }

            lines.Add(@"if NOT [""%errorlevel%""]==[""0""] pause");

            File.WriteAllText(BatFileLocationInfo.SharedDir + "All.bat", string.Join(Environment.NewLine, lines));
        }
        #endregion
    }
}